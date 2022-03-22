using ClinicProject.Shared.Attributes;
using System.Text.Json.Serialization;

namespace ClinicProject.Shared.DTOs
{
    public abstract class DTOBase
    {
        public DTOBase()
        {
            CreationDate = DateTime.UtcNow;
            UpdateDate = DateTime.UtcNow;
        }

        [DataField(DisplayName = "Id", DataField = DataField.Empty, EditPreview = true, ServerSearchable = true)]
        public int Id { get; set; }

        [DataField(DisplayName = "Creation Date", DataField = DataField.DateTime, ClientSearchable = true)]
        public virtual DateTime CreationDate { get; set; }

        [DataField(DisplayName = "Update Date", DataField = DataField.DateTime, ClientSearchable = true)]
        public virtual DateTime UpdateDate { get; set; }

        [JsonIgnore]
        private IndexedProperty<string, object>? _objValues;

        [JsonIgnore]
        public IndexedProperty<string, object> ObjectValues
        {
            get
            {
                if (_objValues == null)
                {
                    _objValues = new IndexedProperty<string, object>(
                        (index) => { return this.GetType().GetProperty(index).GetValue(this); },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _objValues;
            }
        }

        [JsonIgnore]
        private IndexedProperty<string, string>? _strValues;

        [JsonIgnore]
        public IndexedProperty<string, string> StringValues
        {
            get
            {
                if (_strValues == null)
                {
                    _strValues = new IndexedProperty<string, string>(
                        (index) => { return this.GetType().GetProperty(index).GetValue(this) as string; },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _strValues;
            }
        }

        [JsonIgnore]
        private IndexedProperty<string, int>? _intValues;

        [JsonIgnore]
        public IndexedProperty<string, int> IntValues
        {
            get
            {
                if (_intValues == null)
                {
                    _intValues = new IndexedProperty<string, int>(
                        (index) => { return (int)this.GetType().GetProperty(index).GetValue(this); },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _intValues;
            }
        }

        [JsonIgnore]
        private IndexedProperty<string, DateTime?>? _dateValues;

        [JsonIgnore]
        public IndexedProperty<string, DateTime?> DateValues
        {
            get
            {
                if (_dateValues == null)
                {
                    _dateValues = new IndexedProperty<string, DateTime?>(
                        (index) => { return Convert.ToDateTime(this.GetType().GetProperty(index).GetValue(this)); },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, (DateTime)value); });
                }

                return _dateValues;
            }
        }
    }
}
