using ClinicProject.Server.Data.DBModels.ModelBaseTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Shared.Models.Appointment;

namespace ClinicProject.Server.Data.DBModels.AppointmentsTypes
{
    public class Appointment : DBEntityBase
    {
        public virtual AppointmentType AppointmentType { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual Patient Patient { get; set; }
    }
}