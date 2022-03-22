using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.Models.Appointment;
using System.Text.Json.Serialization;

namespace ClinicProject.Shared.DTOs.Appointments
{
    public class AppointmentDTO : DTOBase
    {
        public AppointmentDTO()
        {
            AppointmentType = AppointmentType.Type1;
            Date = DateTime.UtcNow;
        }

        [DataField(DisplayName = "Appointment Type", DataField = DataField.Enum, Editable = true, ClientSearchable = true)]
        public virtual AppointmentType AppointmentType { get; set; }

        [DataField(DisplayName = "Date", DataField = DataField.DateTime, Editable = true, ClientSearchable = true)]
        public virtual DateTime Date { get; set; }

        [JsonIgnore]
        [DataField(DisplayName = "First Name", DataField = DataField.TextNavigationExpanded, ClientSearchable = true, ExpandedFrom = nameof(Patient))]
        public string? FirstName { get { return Patient?.FirstName; } set { if (Patient != null) { Patient.FirstName = value; } } }

        [JsonIgnore]
        [DataField(DisplayName = "Last Name", DataField = DataField.TextNavigationExpanded, ClientSearchable = true, ExpandedFrom = nameof(Patient))]
        public string? LastName { get { return Patient?.LastName; } set { if (Patient != null) { Patient.LastName = value; } } }

        [DataField(DisplayName = "Patient", DataField = DataField.Navigation, Expanded = true)]
        public PatientDTO? Patient { get; set; }

        [DataField(DisplayName = "Patient Id")]
        public int PatientId { get; set; }
    }
}