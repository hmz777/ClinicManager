using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.Models.Patient;

namespace ClinicProject.Shared.DTOs.Patients
{
    public class PatientDTO : DTOBase
    {
        public PatientDTO()
        {
            Gender = Gender.Male;
        }

        [DataField(DisplayName = "First Name", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public string? FirstName { get; set; }

        [DataField(DisplayName = "Middle Name", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public string? MiddleName { get; set; }

        [DataField(DisplayName = "Last Name", DataField = DataField.Text, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public string? LastName { get; set; }

        [DataField(DisplayName = "Age", DataField = DataField.Number, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public int Age { get; set; }

        [DataField(DisplayName = "Gender", DataField = DataField.Enum, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public Gender Gender { get; set; }

        [DataField(DisplayName = "Phone Number", DataField = DataField.PhoneNumber, Editable = true, ClientSearchable = true, ServerSearchable = true)]
        public string? PhoneNumber { get; set; }

        [DataField(DisplayName = "Extra Data", DataField = DataField.Navigation)]
        public ExtraDataDTO? ExtraData { get; set; }

        [DataField(DisplayName = "Appointments", DataField = DataField.Navigation)]
        public List<AppointmentDTO>? Appointments { get; set; }

        [DataField(DisplayName = "Treatments", DataField = DataField.Navigation)]
        public List<TreatmentDTO>? Treatments { get; set; }

        [DataField(DisplayName = "Notes", DataField = DataField.Navigation)]
        public List<NoteDTO>? Notes { get; set; }
    }
}