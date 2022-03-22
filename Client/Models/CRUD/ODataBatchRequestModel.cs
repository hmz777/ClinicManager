using ClinicProject.Client.Services;
using ClinicProject.Shared.DTOs;
using System.Text.Json.Serialization;

namespace ClinicProject.Client.Models.CRUD
{
    public class ODataBatchRequestModel<T> where T : DTOBase
    {
        public List<ODataBatchRequest<T>> Requests { get; set; } = new();

        public bool HasRequests()
        {
            return Requests != null && Requests.Count > 0;
        }
    }

    public class ODataBatchRequest<T> where T : DTOBase
    {
        public ODataBatchRequest(HttpMethod httpMethod, T body, object key)
        {
            RequestId = Guid.NewGuid();
            Url = APIDiscoveryService.APIDefinitionDocument.GetEndpoint<T>();
            HttpMethod = httpMethod.Method;
            Body = body;
            Key = key;

            switch (httpMethod)
            {
                case HttpMethod m when m == System.Net.Http.HttpMethod.Get:
                    {
                        Headers = new() { ["Accept"] = "application/json", ["Content-Type"] = "application/json" };
                        break;
                    }
                case HttpMethod m when m == System.Net.Http.HttpMethod.Post:
                    {
                        Headers = new() { ["Accept"] = "application/json", ["Content-Type"] = "application/json;odata.metadata=minimal" };
                        break;
                    }
                case HttpMethod m when m == System.Net.Http.HttpMethod.Put:
                case HttpMethod mm when mm == System.Net.Http.HttpMethod.Delete:
                    {
                        Headers = new() { ["Accept"] = "application/json", ["Content-Type"] = "application/json;odata.metadata=minimal" };

                        if (key != null)
                        {
                            Url += "/" + key;
                        }

                        Body.UpdateDate = DateTime.UtcNow;

                        break;
                    }
                default:
                    {
                        Headers = new();
                        break;
                    }
            }
        }

        [JsonPropertyName("id")]
        public Guid RequestId { get; }
        [JsonPropertyName("url")]
        public string Url { get; }
        [JsonPropertyName("method")]
        public string HttpMethod { get; }
        [JsonPropertyName("body")]
        public T Body { get; set; }
        [JsonPropertyName("headers")]
        public Dictionary<string, string>? Headers { get; }
        [JsonIgnore]
        public object Key { get; set; }
    }
}