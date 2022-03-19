using AutoMapper;
using ClinicProject.Client.Models.CRUD;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.Models.Error;
using Microsoft.Extensions.Options;
using MudBlazor;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ClinicProject.Client.Services
{
    public class ODataCRUDHandler<T> where T : DTOBase
    {
        private readonly HttpClient httpClient;
        private readonly APIDiscoveryService discoveryService;
        private readonly IMapper mapper;
        private readonly IOptions<JsonSerializerOptions> jsonOptions;

        public ODataCRUDHandler(HttpClient httpClient, APIDiscoveryService discoveryService, IMapper mapper, IOptions<JsonSerializerOptions> jsonOptions)
        {
            this.httpClient = httpClient;
            this.discoveryService = discoveryService;
            this.mapper = mapper;
            this.jsonOptions = jsonOptions;
        }

        public async Task<KeyValuePair<int, IEnumerable<T>>> Get(CRUDModel crudModel)
        {
            var fullUrl = ConstructUrl(ConstructQuery(crudModel));

            var json = await httpClient.GetFromJsonAsync<JsonDocument>(fullUrl);

            var Elements = json?
             .RootElement
             .GetProperty("value")
             .Deserialize<List<T>>(jsonOptions.Value)
             ?? Enumerable.Empty<T>().ToList();

            var count = json?.RootElement.GetProperty("@odata.count").GetInt32() ?? 0;

            return new KeyValuePair<int, IEnumerable<T>>(count, Elements);
        }

        public async Task<KeyValuePair<HttpStatusCode, ModelValidationResult?>> Put(T Item)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, discoveryService.DiscoveryEndPoint +
            APIDiscoveryService.APIDefinitionDocument.GetEndpoint<T>() + $"({Item.Id})");

            request.Headers.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = mapper.Map<T>(Item);

            var content = new StringContent(JsonSerializer.Serialize<T>(data), Encoding.UTF8, "application/json");
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/json;odata.metadata=minimal");

            request.Content = content;
            var res = await httpClient.SendAsync(request);

            ModelValidationResult? result = null;

            if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                var bodyResult = await res.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(bodyResult))
                {
                    result = JsonSerializer.Deserialize<ModelValidationResult>(bodyResult, jsonOptions.Value);
                }
            }

            return new KeyValuePair<HttpStatusCode, ModelValidationResult?>(res.StatusCode, result);
        }

        public async Task<KeyValuePair<HttpStatusCode, ModelValidationResult?>> Post(T Item)
        {
            var res = await httpClient.PostAsJsonAsync<T>(discoveryService.DiscoveryEndPoint +
                 APIDiscoveryService.APIDefinitionDocument.GetEndpoint<T>(), Item);

            ModelValidationResult? result = null;

            if (res.StatusCode == HttpStatusCode.BadRequest)
            {
                var bodyResult = await res.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(bodyResult))
                {
                    result = JsonSerializer.Deserialize<ModelValidationResult>(bodyResult);
                }
            }

            return new KeyValuePair<HttpStatusCode, ModelValidationResult?>(res.StatusCode, result);
        }

        public async Task<HttpStatusCode> Delete(int Id)
        {
            var res = await httpClient.DeleteAsync(discoveryService.DiscoveryEndPoint +
                 APIDiscoveryService.APIDefinitionDocument.GetEndpoint<T>() + $"({Id})");

            return res.StatusCode;
        }

        public async Task<ODataBatchResponseModel> Batch(ODataBatchRequestModel<T> batchModel)
        {
            var res = await httpClient.PostAsJsonAsync<ODataBatchRequestModel<T>>(
                discoveryService.BatchingEndpoint + "$batch"
                , batchModel);

            return await res.Content.ReadFromJsonAsync<ODataBatchResponseModel>();
        }

        static string ConstructQuery(CRUDModel crudModel)
        {
            string query = "?";

            if (crudModel.HasSearch())
            {
                query += $"$search=\"{crudModel.SearchString}\"&";
            }

            if (crudModel.HasFilter())
            {
                string filter = "$filter=";

                if (crudModel.EqFilters != null)
                {
                    foreach (var eqFilter in crudModel.EqFilters)
                    {
                        if (eqFilter.Value == null)
                            continue;

                        if (eqFilter.Value.Item1 is DateRange dateRange)
                        {
                            //We have a date range

                            filter += $"({eqFilter.Key} {ODataOperand.gt} {dateRange.Start.Value:yyyy-MM-ddTHH:mm:ssZ} and " +
                                $"{eqFilter.Key} {ODataOperand.lt} {dateRange.End.Value:yyyy-MM-ddTHH:mm:ssZ}) {eqFilter.Value.Item3} ";
                        }
                        else
                        {
                            switch (eqFilter.Value.Item2)
                            {
                                case ODataFilterOp.Equal:
                                    filter += $"{eqFilter.Key} eq {eqFilter.Value.Item1} {eqFilter.Value.Item3} ";
                                    break;
                                case ODataFilterOp.Contains:
                                    filter += $"contains({eqFilter.Key}, {eqFilter.Value.Item1}) {eqFilter.Value.Item3} ";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (filter.EndsWith(" "))
                {
                    //Remove the trailing operand
                    filter = filter.Remove(filter[0..^1].LastIndexOf(" "));
                }

                if (!filter.EndsWith('='))
                {
                    filter += "&";
                    query += filter;
                }
            }

            if (!string.IsNullOrWhiteSpace(crudModel.SortLabel))
            {
                query += $"$orderby={crudModel.SortLabel} {(crudModel.SortDirection == Models.CRUD.SortDirection.none ? Models.CRUD.SortDirection.asc : crudModel.SortDirection)}&";
            }
            else
            {
                query += $"$orderby=Id asc&";
            }

            if (crudModel.PageSize > 0)
            {
                query += $"$top={crudModel.PageSize}&";

                if (crudModel.Page >= 0)
                {
                    query += $"$skip={crudModel.Page * crudModel.PageSize}&";
                }
            }

            if (crudModel.HasSelect())
            {
                var select = "$select=" + string.Join(',', crudModel.SelectedProperties);

                if (!select.EndsWith('='))
                {
                    select += "&";
                    query += select;
                }
            }

            if (crudModel.HasExpand())
            {
                var expand = "$expand=";

                foreach (var item in crudModel.ExpandedProperties)
                {
                    expand += $"{item} and ";
                }

                if (expand.EndsWith("and "))
                {
                    expand = expand.Remove(expand.Length - 4);
                }

                if (!expand.EndsWith('='))
                {
                    expand += "&";
                    query += expand;
                }
            }

            query += $"$count=true";

            return query;
        }

        string ConstructUrl(string query)
            => discoveryService.DiscoveryEndPoint + APIDiscoveryService.APIDefinitionDocument.GetEndpoint<T>() + query;
    }
}
