using ClinicProject.Client.Models.CRUD;
using ClinicProject.Client.Services;
using ClinicProject.Client.Shared.Patients;
using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.Models.Error;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;

namespace ClinicProject.Client.Shared.Components
{
    public partial class ODataCRUDBase<T> where T : DTOBase
    {
        #region Services

        [Inject] ODataCRUDHandler<T> ODataCRUDHandler { get; set; }

        #endregion

        #region Members

        Dictionary<PropertyInfo, (DisplayAttribute, DataFieldAttribute)> PropertyAttributes = new();

        MudTable<T>? Table;
        IEnumerable<T> Items = new HashSet<T>();
        IEnumerable<T> BackupItems = new HashSet<T>();
        HashSet<T> SelectedItems = new();

        string? searchString;
        T? EditBackupItem;

        bool CanDelete = false;
        bool CanSelectAll = true;
        bool CanClearSelection = false;

        DateTime DateFrom { get; set; }
        DateTime DateTo { get; set; }

        public ODataBatchRequestModel<T> BatchModel { get; set; } = new();

        #endregion

        #region Life Cycle

        protected override void OnInitialized()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var dataField = property.GetCustomAttributes(typeof(DataFieldAttribute), true).Cast<DataFieldAttribute>().FirstOrDefault();
                var disp = property.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>().FirstOrDefault();

                PropertyAttributes[property] = (disp, dataField);
            }

            PropertyAttributes = PropertyAttributes
              .OrderByDescending(p => p.Key.Name.ToLower().StartsWith('i'))
              .ThenByDescending(p => p.Value.Item2?.DataField != DataField.Navigation)
              .ToDictionary(p => p.Key, p => p.Value);
        }

        #endregion

        #region Table Selection Events

        void SelectedItemsChanged(HashSet<T> selectedItems)
        {
            CanDelete = SelectedItems?.Count > 0;
            CanSelectAll = SelectedItems?.Count == 0;
            CanClearSelection = SelectedItems?.Count > 0;
        }

        void SelectAll()
        {
            SelectedItems = Items.ToHashSet<T>();
        }

        void ClearSelection()
        {
            SelectedItems.Clear();
        }

        #endregion

        #region Batch Commit Handlers

        void ResetChanges()
        {
            ClearSelection();
            SelectedItemsChanged(null);

            BatchModel.Requests.Clear();

            Mapper.Map(BackupItems, Items);

            StateHasChanged();
        }

        async void SaveChanges()
        {
            var response = await ODataCRUDHandler.Batch(BatchModel);

            var hasSuccess = VisualizeResults(response);

            ResetChanges();

            if (hasSuccess)
            {
                await Table.ReloadServerData();
            }
        }

        #endregion

        #region Batch CRUD

        async void OnAddPatientBatchPart()
        {
            var AddDialog = DialogService.Show<AddPatientDialog>("Add Patient");
            var dialogResult = await AddDialog.Result;

            if (!dialogResult.Cancelled)
            {
                T Data = dialogResult.Data as T;

                BatchModel.Requests.Add(new ODataBatchRequest<T>(HttpMethod.Post, Data, Data.Id));

                StateHasChanged();
            }
        }

        void OnDeleteBatchPart()
        {
            foreach (var item in SelectedItems)
            {
                if (BatchModel.Requests.Any(r => r.Key.Equals(item.Id)))
                {
                    return;
                }

                BatchModel.Requests.Add(new ODataBatchRequest<T>(HttpMethod.Delete, item, item.Id));
            }

            StateHasChanged();
        }

        void OnBatchPart(object item)
        {
            T tItem = item as T;

            var request = BatchModel.Requests.FirstOrDefault(r => r.Key.Equals(tItem.Id));

            if (request == null)
            {
                BatchModel.Requests.Add(new ODataBatchRequest<T>(HttpMethod.Put, tItem, tItem.Id));
            }
            else
            {
                request.Body = tItem;
            }

            StateHasChanged();
        }

        #endregion

        #region Inline Editing

        void ItemBackup(object item)
        {
            EditBackupItem = Mapper.Map<T>(item);
        }

        void OnItemEditCancel(object item)
        {
            Mapper.Map(EditBackupItem, item);
        }

        #endregion

        #region Table Serverside Processing

        async Task<TableData<T>> ServerReload(TableState tableState)
        {
            ClearSelection();

            var crudModel = Mapper.Map<CRUDModel>(tableState);
            crudModel.From = DateFrom;
            crudModel.To = DateTo;
            crudModel.SearchString = searchString;

            var res = await ODataCRUDHandler.Get(crudModel);
            Items = res.Value;
            BackupItems = Mapper.Map<IEnumerable<T>>(Items);

            return new TableData<T>()
            {
                TotalItems = res.Key,
                Items = Items
            };
        }

        #endregion

        #region Table Search

        async void OnSearch(string text)
        {
            searchString = text;
            Table.NavigateTo(Page.First);
            await Table.ReloadServerData();
        }

        #endregion

        #region Tools

        bool VisualizeResults(ODataBatchResponseModel responseModel)
        {
            bool hasSuccessFlag = false;

            var responses = BatchModel.Requests.Select(req => new { Id = req.RequestId, req.HttpMethod })
                .Join(responseModel.Responses.Select(res => new { Id = res.ResponseId, res.Status, res.Body })
                , req => req.Id, res => res.Id, (req, res) =>
                new { req.HttpMethod, res.Status, Messages = res.Body?.Deserialize<ModelValidationResult>() });

            foreach (var response in responses)
            {
                switch (response.Status)
                {
                    case System.Net.HttpStatusCode.OK:
                        Snackbar.Add("Request completed successfully.", Severity.Success);
                        hasSuccessFlag = true;
                        break;
                    case System.Net.HttpStatusCode.Created:
                        Snackbar.Add("Resource created successfully.", Severity.Success);
                        hasSuccessFlag = true;
                        break;
                    case System.Net.HttpStatusCode.NoContent:
                        Snackbar.Add("Resource updated successfully.", Severity.Success);
                        hasSuccessFlag = true;
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        Snackbar.Add($"{response.Messages}", Severity.Error);
                        break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        Snackbar.Add($"Internal server error.", Severity.Error);
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        Snackbar.Add($"Resource not found.", Severity.Error);
                        break;
                    default:
                        break;
                }
            }

            return hasSuccessFlag;
        }

        #endregion
    }
}