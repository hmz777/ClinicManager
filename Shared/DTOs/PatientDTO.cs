using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.Models.Patient;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject.Shared.DTOs
{
    public class PatientDTO : DTOBase
    {
        [Display(Name = "First Name")]
        [DataField(DataField.Text, true)]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string? FirstName { get; set; }
        [Display(Name = "Middle Name")]
        [DataField(DataField.Text, true)]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string? MiddleName { get; set; }
        [Display(Name = "Last Name")]
        [DataField(DataField.Text, true)]
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string? LastName { get; set; }
        [Display(Name = "Age")]
        [DataField(DataField.Number, true)]
        [Required]
        [Range(1, 110)]
        public int Age { get; set; }
        [Display(Name = "Gender")]
        [DataField(DataField.Enum, true)]
        [Required]
        public Gender Gender { get; set; }
        [Display(Name = "Phone Number")]
        [DataField(DataField.PhoneNumber, true)]
        [Required]
        [StringLength(15)]
        public string? PhoneNumber { get; set; }
        [Display(Name = "Extra Data")]
        [DataField(DataField.Navigation)]
        public ExtraDataDTO? ExtraData { get; set; }
        [Display(Name = "Appointments")]
        [DataField(DataField.Navigation)]
        public List<AppointmentDTO>? Appointments { get; set; }
        [Display(Name = "Treatments")]
        [DataField(DataField.Navigation)]
        public List<TreatmentDTO>? Treatments { get; set; }
        [Display(Name = "Notes")]
        [DataField(DataField.Navigation)]
        public List<NoteDTO>? Notes { get; set; }
    }
}
