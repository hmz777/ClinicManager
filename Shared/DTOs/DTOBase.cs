using ClinicProject.Shared.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClinicProject.Shared.DTOs
{
    public abstract class DTOBase
    {
        [Display(Name = "Id")]
        [DataField(DataField.Empty)]
        public int Id { get; set; }
        [Display(Name = "Creation Date")]
        [DataField(DataField.DateTime)]
        public virtual DateTime CreationDate { get; set; }
        [Display(Name = "Update Date")]
        [DataField(DataField.DateTime)]
        public virtual DateTime UpdateDate { get; set; }

        [NotMapped]
        [JsonPropertyName("@odata.type")]
        public string ODataType { get; set; } = string.Empty;
    }
}
