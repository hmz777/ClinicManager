using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClinicProject.Client.Models.CRUD
{
    public class ODataBatchResponseModel
    {
        public IEnumerable<ODataBatchResponse> Responses { get; set; }
    }

    public class ODataBatchResponse
    {
        [JsonPropertyName("id")]
        public Guid ResponseId { get; set; }
        [JsonPropertyName("status")]
        public HttpStatusCode Status { get; set; }
        [JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; }
        [JsonPropertyName("body")]
        public JsonDocument Body { get; set; }
    }
}