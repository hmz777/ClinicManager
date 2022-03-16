using ClinicProject.Shared.Attributes;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.Models.Appointment;

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

        [DataField(DisplayName = "Patient", DataField = DataField.Navigation)]
        public PatientDTO? PatientDTO { get; set; }
    }
}