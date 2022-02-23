using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.ModelBaseTypes;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using ClinicProject.Shared.Models.Patient;

namespace ClinicProject.Server.Data.DBModels.PatientTypes
{
    public class Patient : DBEntityBase
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
            Notes = new HashSet<Note>();
            Treatments = new HashSet<Treatment>();
        }

        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public virtual Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public ExtraData? ExtraData { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
    }
}
