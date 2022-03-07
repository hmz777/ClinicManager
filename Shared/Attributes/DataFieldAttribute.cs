namespace ClinicProject.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DataFieldAttribute : Attribute
    {
        public DataFieldAttribute(DataField DataField, bool Editable = false, bool EditPreview = false)
        {
            this.DataField = DataField;
            this.Editable = Editable;
        }


        public DataField DataField { get; }
        public bool Editable { get; set; }
        public bool EditPreview { get; set; }
    }
}