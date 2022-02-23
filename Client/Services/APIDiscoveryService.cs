using ClinicProject.Client.Models;
using System.Net.Http.Json;

namespace ClinicProject.Client.Services
{
    public class APIDiscoveryService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;

        public APIDiscoveryService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            DiscoveryEndPoint = this.configuration.GetSection("OData").Value;

            HttpClient = this.httpClientFactory.CreateClient("NonTokenClient");
        }

        HttpClient HttpClient { get; set; }

        public string? DiscoveryEndPoint { get; }

        public static APIDefinitionDocument APIDefinitionDocument { get; set; } = new();

        public async Task Discover()
        {
            var doc = await HttpClient.GetFromJsonAsync<ODataDescription>(DiscoveryEndPoint);

            if (doc == null || doc == default(ODataDescription))
                throw new Exception("No valid endpoint found!");

            var endpoints = doc.value.Select(m => m.url).ToList();

            foreach (var property in APIDefinitionDocument.GetType().GetProperties())
            {
                property.SetValue(APIDefinitionDocument, "/" + endpoints?.Where(e => e.ToLower() == property.Name.ToLower()).FirstOrDefault());
            }
        }
    }
}
