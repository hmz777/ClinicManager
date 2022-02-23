using ClinicProject.Shared.Models.Appointment;

namespace ClinicProject.Shared.DTOs
{
    public class AppointmentDTO : DTOBase
    {
        public virtual AppointmentType AppointmentType { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
