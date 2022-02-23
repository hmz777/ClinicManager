namespace ClinicProject.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DataFieldAttribute : Attribute
    {
        public DataFieldAttribute(DataField DataField)
        {
            this.DataField = DataField;
        }

        public DataField DataField { get; }
    }
}