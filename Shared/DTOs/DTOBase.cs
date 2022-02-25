using ClinicProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicProject.Shared.DTOs
{
    public abstract class DTOBase
    {
        [Display(Name = "Id")]
        [DataField(DataField.Empty, EditPreview = true)]
        public int Id { get; set; }
        [Display(Name = "Creation Date")]
        [DataField(DataField.DateTime, EditPreview = true)]
        public virtual DateTime CreationDate { get; set; }
        [Display(Name = "Update Date")]
        [DataField(DataField.DateTime, EditPreview = true)]
        public virtual DateTime UpdateDate { get; set; }

        [NotMapped]
        [JsonPropertyName("@odata.type")]
        public string ODataType { get; set; } = string.Empty;

        private IndexedProperty<string, object> _objValues;

        [NotMapped]
        public IndexedProperty<string, object> ObjectValues
        {
            get
            {
                if (_objValues == null)
                {
                    return new IndexedProperty<string, object>(
                        (index) => { return this.GetType().GetProperty(index).GetValue(this); },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _objValues;
            }
        }

        private IndexedProperty<string, string> _strValues;

        [NotMapped]
        public IndexedProperty<string, string> StringValues
        {
            get
            {
                if (_strValues == null)
                {
                    return new IndexedProperty<string, string>(
                        (index) => { return this.GetType().GetProperty(index).GetValue(this) as string; },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _strValues;
            }
        }

        private IndexedProperty<string, int> _intValues;

        [NotMapped]
        public IndexedProperty<string, int> IntValues
        {
            get
            {
                if (_intValues == null)
                {
                    return new IndexedProperty<string, int>(
                        (index) => { return (int)this.GetType().GetProperty(index).GetValue(this); },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _intValues;
            }
        }

        private IndexedProperty<string, DateTime?> _dateValues;

        [NotMapped]
        public IndexedProperty<string, DateTime?> DateValues
        {
            get
            {
                if (_dateValues == null)
                {
                    return new IndexedProperty<string, DateTime?>(
                        (index) => { return Convert.ToDateTime(this.GetType().GetProperty(index).GetValue(this)); },
                        (index, value) => { this.GetType().GetProperty(index).SetValue(this, value); });
                }

                return _dateValues;
            }
        }
    }
}
