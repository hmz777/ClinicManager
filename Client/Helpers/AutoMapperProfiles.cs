using AutoMapper;
using ClinicProject.Client.Models.CRUD;
using ClinicProject.Client.Models.Patients;
using ClinicProject.Shared.DTOs;
using ClinicProject.Shared.DTOs.Appointments;
using ClinicProject.Shared.DTOs.Patients;
using ClinicProject.Shared.DTOs.Treatments;
using MudBlazor;

namespace ClinicProject.Client.Helpers
{
    public class AutoMapperProfiles
    {
        class DTOBaseMaps : Profile
        {
            public DTOBaseMaps()
            {
                CreateMap<DTOBase, DTOBase>()
                    .ForMember(d => d.StringValues, opt => opt.Ignore())
                    .ForMember(d => d.IntValues, opt => opt.Ignore())
                    .ForMember(d => d.DateValues, opt => opt.Ignore())
                    .ForMember(d => d.ObjectValues, opt => opt.Ignore())
                    .ForMember(d => d.DecimalValues, opt => opt.Ignore());
            }
        }

        class PatientDTOMaps : Profile
        {
            public PatientDTOMaps()
            {
                CreateMap<PatientDTO, PatientDTO>()
                    .IncludeBase<DTOBase, DTOBase>();

                CreateMap<PatientDTO, PatientSelectModel>();
            }
        }

        class AppointmentDTOMaps : Profile
        {
            public AppointmentDTOMaps()
            {
                CreateMap<AppointmentDTO, AppointmentDTO>()
                    .ForMember(d => d.FirstName, opt => opt.Ignore())
                    .ForMember(d => d.LastName, opt => opt.Ignore());
            }
        }

        class TreatmentDTOMaps : Profile
        {
            public TreatmentDTOMaps()
            {
                CreateMap<TreatmentDTO, TreatmentDTO>();
            }
        }

        class CRUDMaps : Profile
        {
            public CRUDMaps()
            {
                CreateMap<TableState, CRUDModel>()
                   .ForMember(d => d.SortDirection, opt => opt.MapFrom(s => s.SortDirection));
            }
        }
    }
}
