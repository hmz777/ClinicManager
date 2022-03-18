using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.Models.Appointment;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicProject.Shared.DTOs.Appointments
{
    public class AppointmentDTO : DTOBase
    {
        public AppointmentDTO()
        {
            AppointmentType = AppointmentType.Type1;
            Date = DateTime.UtcNow;
        }

        [DataField(DisplayName = "Appointment Type", DataField = DataField.Enum, Editable = true, Searchable = true)]
        public virtual AppointmentType AppointmentType { get; set; }

        [DataField(DisplayName = "Date", DataField = DataField.DateTime, Editable = true, Searchable = true)]
        public virtual DateTime Date { get; set; }

        [NotMapped]
        [DataField(DisplayName = "First Name", DataField = DataField.TextNavigationExpanded, Searchable = true, ExpandedFrom = nameof(Patient))]
        public string FirstName { get { return Patient.FirstName; } set { Patient.FirstName = value; } }

        [NotMapped]
        [DataField(DisplayName = "Last Name", DataField = DataField.TextNavigationExpanded, Searchable = true, ExpandedFrom = nameof(Patient))]
        public string LastName { get { return Patient.LastName; } set { Patient.LastName = value; } }

        [DataField(DisplayName = "Patient", DataField = DataField.Navigation, Expanded = true)]
        public PatientDTO? Patient { get; set; }
    }
}