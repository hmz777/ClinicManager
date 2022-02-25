using AutoMapper;
using ClinicProject.Client.Models.CRUD;
using ClinicProject.Shared.DTOs;
using MudBlazor;

namespace ClinicProject.Client.Helpers
{
    public class AutoMapperProfiles
    {
        class Patient2Patient : Profile
        {
            public Patient2Patient()
            {
                CreateMap<PatientDTO, PatientDTO>()
                    .ForMember(d => d.StringValues, opt => opt.Ignore())
                    .ForMember(d => d.IntValues, opt => opt.Ignore())
                    .ForMember(d => d.DateValues, opt => opt.Ignore())
                    .ForMember(d => d.ObjectValues, opt => opt.Ignore());
            }
        }

        class TableState2CRUDModel : Profile
        {
            public TableState2CRUDModel()
            {
                CreateMap<TableState, CRUDModel>()
                   .ForMember(d => d.SortDirection, opt => opt.MapFrom(s => s.SortDirection));
            }
        }
    }
}
