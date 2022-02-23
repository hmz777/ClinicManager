using ClinicProject.Shared.DTOs;

namespace ClinicProject.Client.Models
{
    public class APIDefinitionDocument
    {
        public string? Patients { get; set; }
        public string? Appointments { get; set; }
        public string? Treatments { get; set; }
        public string? Payments { get; set; }
        public string? ExtraData { get; set; }
        public string? Notes { get; set; }

        public string GetEndpoint<T>()
        {
            switch (typeof(T))
            {
                case Type type when type == typeof(PatientDTO):
                    return Patients;
                case Type type when type == typeof(AppointmentDTO):
                    return Appointments;
                case Type type when type == typeof(TreatmentDTO):
                    return Treatments;
                case Type type when type == typeof(PaymentDTO):
                    return Payments;
                case Type type when type == typeof(ExtraDataDTO):
                    return ExtraData;
                case Type type when type == typeof(NoteDTO):
                    return Notes;

                default:
                    throw new NotImplementedException("There isnt a valid endpoint for the specified type.");
            }
        }

        public bool IsValid() =>
            Patients != null
            && Appointments != null
            && Treatments != null
            && Payments != null
            && ExtraData != null;
    }
}
