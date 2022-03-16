using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.DTOs.Patients;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ClinicProject.Server.OData
{
    public class OdataModelBuilder
    {
        public IEdmModel GetEDM()
        {
            ODataConventionModelBuilder builder = new();

            builder.EntitySet<PatientDTO>((nameof(Patient) + "s"));
            builder.EntitySet<AppointmentDTO>((nameof(Appointment) + "s"));
            builder.EntitySet<TreatmentDTO>((nameof(Treatment) + "s"));
            builder.EntitySet<PaymentDTO>((nameof(Payment) + "s"));
            builder.EntitySet<NoteDTO>((nameof(Note) + "s"));
            builder.EntitySet<ExtraDataDTO>(nameof(ExtraData));

            return builder.GetEdmModel();
        }
    }
}