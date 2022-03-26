using AutoMapper;
using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.ModelBaseTypes;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.DTOs.Treatments;
using ClinicProject.Shared.Payments;

namespace ClinicProject.Server.Helpers
{
    public class AutoMapperProfiles
    {
        public class BaseMaps : Profile
        {
            public BaseMaps()
            {
                CreateMap<DBEntityBase, DTOBase>().ReverseMap();
            }
        }

        public class PatientMaps : Profile
        {
            public PatientMaps()
            {
                CreateMap<Patient, PatientDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>()
                    .ForMember(s => s.Appointments, opt => opt.ExplicitExpansion())
                    .ForMember(s => s.Notes, opt => opt.ExplicitExpansion())
                    .ForMember(s => s.ExtraData, opt => opt.ExplicitExpansion())
                    .ForMember(s => s.Treatments, opt => opt.ExplicitExpansion())
                    .ReverseMap()
                    .IncludeBase<DTOBase, DBEntityBase>()
                    .ForMember(s => s.Appointments, opt => opt.Ignore())
                    .ForMember(s => s.Notes, opt => opt.Ignore())
                    .ForMember(s => s.ExtraData, opt => opt.Ignore())
                    .ForMember(s => s.Treatments, opt => opt.Ignore());
            }
        }

        public class AppointmentMaps : Profile
        {
            public AppointmentMaps()
            {
                CreateMap<Appointment, AppointmentDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>()
                    .ForMember(s => s.Patient, opt => opt.ExplicitExpansion())
                    .ReverseMap();
            }
        }

        public class TreatmentMaps : Profile
        {
            public TreatmentMaps()
            {
                CreateMap<Treatment, TreatmentDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>()
                    .ForMember(s => s.Patient, opt => opt.ExplicitExpansion())
                    .ForMember(s => s.Payments, opt => opt.ExplicitExpansion())
                    .ReverseMap();
            }
        }

        public class PaymentMaps : Profile
        {
            public PaymentMaps()
            {
                CreateMap<Payment, PaymentDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }

        public class NoteMaps : Profile
        {
            public NoteMaps()
            {
                CreateMap<Note, NoteDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }

        public class ExtraDataPMaps : Profile
        {
            public ExtraDataPMaps()
            {
                CreateMap<ExtraData, ExtraDataDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }
    }
}
