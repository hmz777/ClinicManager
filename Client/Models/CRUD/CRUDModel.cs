namespace ClinicProject.Client.Models.CRUD
{
    public class CRUDModel
    {
        public string SearchString { get; set; }
        public string SortLabel { get; set; }
        public SortDirection SortDirection { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
