namespace ClinicProject.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DataFieldAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public DataField DataField { get; set; }
        public bool Editable { get; set; }
        public bool EditPreview { get; set; }
        public bool ClientSearchable { get; set; }
        public bool ServerSearchable { get; set; }
        public bool Expanded { get; set; }
        public string ExpandedFrom { get; set; }
    }
}