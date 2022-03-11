namespace ClinicProject.Client.Models.CRUD
{
    public class CRUDModel
    {
        public string SearchString { get; set; }
        public string SortLabel { get; set; }
        public SortDirection SortDirection { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedUntil { get; set; }
        public DateTime? UpdatedFrom { get; set; }
        public DateTime? UpdatedUntil { get; set; }

        public Dictionary<string, object> EqFilters { get; set; }

        public bool HasFilter()
        {
            return (EqFilters != null && EqFilters.Count > 0)
                || CreatedFrom != default
                || CreatedUntil != default
                || UpdatedFrom != default
                || UpdatedUntil != default;
        }

        public bool HasSearch()
        {
            return !string.IsNullOrWhiteSpace(SearchString);
        }
    }
}