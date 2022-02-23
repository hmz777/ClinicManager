namespace ClinicProject.Client.Models.CRUD
{
    public class ODataBatchRequestModel
    {
        public List<ODataBatchRequest> Requests { get; set; }
    }

    public class ODataBatchRequest
    {
        public Guid Id { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public string Url { get; set; }
        public string[] Headers { get; set; }
    }
}