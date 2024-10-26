using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<History, HistoryDto>();

            CreateMap<CreateHistoryDto, History>();

            CreateMap<UpdateHistoryDto, History>()
                .ForMember(h => h.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
