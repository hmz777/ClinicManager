using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.Models.Patient;
using System.ComponentModel.DataAnnotations;

namespace ClinicProject.Shared.DTOs
{
    public class PatientDTO : DTOBase
    {
        public PatientDTO()
        {
            Gender = Gender.Male;
        }

        [Display(Name = "First Name")]
        [DataField(DataField.Text, Editable = true)]
        public string? FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [DataField(DataField.Text, Editable = true)]
        public string? MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [DataField(DataField.Text, Editable = true)]
        public string? LastName { get; set; }

        [Display(Name = "Age")]
        [DataField(DataField.Number, Editable = true)]
        public int Age { get; set; }

        [Display(Name = "Gender")]
        [DataField(DataField.Enum, Editable = true)]
        public Gender Gender { get; set; }

        [Display(Name = "Phone Number")]
        [DataField(DataField.PhoneNumber, Editable = true)]
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
