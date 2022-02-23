using AutoMapper;
using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.ModelBaseTypes;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using ClinicProject.Shared.DTOs;

namespace ClinicProject.Server.Helpers
{
    public class AutoMapperProfiles
    {
        public class BaseProfile : Profile
        {
            public BaseProfile()
            {
                CreateMap<DBEntityBase, DTOBase>().ReverseMap();
            }
        }

        public class PatientProfile : Profile
        {
            public PatientProfile()
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

        public class AppointmentProfile : Profile
        {
            public AppointmentProfile()
            {
                CreateMap<Appointment, AppointmentDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }

        public class TreatmentProfile : Profile
        {
            public TreatmentProfile()
            {
                CreateMap<Treatment, TreatmentDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>()
                    .ForMember(s => s.Payments, opt => opt.ExplicitExpansion());
            }
        }

        public class PaymentProfile : Profile
        {
            public PaymentProfile()
            {
                CreateMap<Payment, PaymentDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }

        public class NotesProfile : Profile
        {
            public NotesProfile()
            {
                CreateMap<Note, NoteDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }

        public class ExtraDataProfile : Profile
        {
            public ExtraDataProfile()
            {
                CreateMap<ExtraData, ExtraDataDTO>()
                    .IncludeBase<DBEntityBase, DTOBase>();
            }
        }
    }
}
