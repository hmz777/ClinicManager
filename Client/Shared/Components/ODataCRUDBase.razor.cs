using ClinicProject.Client.Models.CRUD;
using ClinicProject.Client.Services;
using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.Models.Error;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Reflection;
using System.Text.Json;

namespace ClinicProject.Client.Shared.Components
{
    public partial class ODataCRUDBase<T> where T : DTOBase
    {
        #region Services

        [Inject] ODataCRUDHandler<T> ODataCRUDHandler { get; set; }
        [Inject] IOptions<JsonSerializerOptions> JsonOptions { get; set; }

        #endregion

        #region Members

        Dictionary<PropertyInfo, DataFieldAttribute> PropertyAttributes = new();

        MudTable<T>? Table;
        IEnumerable<T> Items = new HashSet<T>();
        IEnumerable<T> BackupItems = new HashSet<T>();
        HashSet<T> SelectedItems = new();

        T? EditBackupItem;

        bool CanDelete = false;
        bool CanSelectAll = true;
        bool CanClearSelection = false;

        string? SearchString { get; set; }
        Dictionary<string, object> EqFilters { get; set; } = new();
        DateRange? CreateDateRange { get; set; }
        DateRange? UpdateDateRange { get; set; }

        public Type DialogType { get; set; }

        public ODataBatchRequestModel<T> BatchModel { get; set; } = new();

        #endregion

        #region Life Cycle

        protected override void OnInitialized()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                var attributes = property.GetCustomAttributes();

                var dataField = attributes
                    .Where(a => a.GetType() == typeof(DataFieldAttribute))
                    .Cast<DataFieldAttribute>()
                    .FirstOrDefault();

                PropertyAttributes[property] = dataField;
            }

            PropertyAttributes = PropertyAttributes
              .OrderByDescending(p => p.Key.Name.ToLower().StartsWith('i'))
              .ThenByDescending(p => p.Value?.DataField != DataField.Navigation)
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
            var addDialog = DialogService.Show(DialogType, "Create new");
            var dialogResult = await addDialog.Result;

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

        void OnPutBatchPart(object item)
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

            crudModel.CreatedFrom = CreateDateRange?.Start;
            crudModel.CreatedUntil = CreateDateRange?.End;

            crudModel.UpdatedFrom = UpdateDateRange?.Start;
            crudModel.UpdatedUntil = UpdateDateRange?.End;

            crudModel.SearchString = SearchString;
            crudModel.EqFilters = EqFilters;

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

        void OnSearch(object data, string fieldType)
        {
            if (data == null || fieldType == null)
                return;

            var property = typeof(T).GetProperties().Where(p => p.Name.ToLower() == fieldType.ToLower()).FirstOrDefault();

            if (property != null)
            {
                if (data is string s)
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        EqFilters.Remove(property.Name);
                        return;
                    }

                    data = $"'{s}'";
                }

                EqFilters[property.Name] = data;
            }
            else if (fieldType == "Search")
            {
                SearchString = data.ToString();
            }
        }

        async void DoSearch()
        {
            Table.NavigateTo(Page.First);
            await Table.ReloadServerData();
        }

        #endregion

        #region Date Filter

        void ClearCreateDateRange()
        {
            CreateDateRange = null;
        }

        void ClearUpdateDateRange()
        {
            UpdateDateRange = null;
        }

        #endregion

        #region Tools

        bool VisualizeResults(ODataBatchResponseModel responseModel)
        {
            bool hasSuccessFlag = false;

            var responses = BatchModel.Requests.Select(req => new { Id = req.RequestId, req.HttpMethod })
                .Join(responseModel.Responses.Select(res => new { Id = res.ResponseId, res.Status, res.Body })
                , req => req.Id, res => res.Id, (req, res) =>
                new { req.HttpMethod, res.Status, Messages = res.Body?.Deserialize<ModelValidationResult>(JsonOptions.Value) });

            foreach (var response in responses)
            {
                switch (response.Status)
                {
                    case System.Net.HttpStatusCode.NoContent:
                    case System.Net.HttpStatusCode.OK:
                        Snackbar.Add("Request completed successfully.", Severity.Success);
                        hasSuccessFlag = true;
                        break;
                    case System.Net.HttpStatusCode.Created:
                        Snackbar.Add("Resource created successfully.", Severity.Success);
                        hasSuccessFlag = true;
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        if ((bool)(response.Messages?.HasResults()))
                        {
                            foreach (var msg in response.Messages.Results)
                            {
                                Snackbar.Add($"{msg.Value}", Severity.Error);
                            }
                        }
                        else
                        {
                            Snackbar.Add($"Bad request.", Severity.Error);
                        }
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