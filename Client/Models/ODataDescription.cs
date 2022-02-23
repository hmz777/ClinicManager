using System.Text.Json.Serialization;

namespace ClinicProject.Client.Models
{
    public class ODataDescription
    {
        [JsonPropertyName("@odata.context")]
        public string context { get; set; }
        public IEnumerable<APIManifest> value { get; set; }

        public class APIManifest
        {
            public string name { get; set; }
            public string kind { get; set; }
            public string url { get; set; }
        }
    }
}
