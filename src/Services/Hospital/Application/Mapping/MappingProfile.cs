using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Hospital, HospitalDto>();

            CreateMap<CreateHospitalDto, Hospital>();

            CreateMap<UpdateHospitalDto, Hospital>()
                .ForMember(h => h.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
