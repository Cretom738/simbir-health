using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Timetable, TimetableDto>();

            CreateMap<CreateTimetableDto, Timetable>();

            CreateMap<UpdateTimetableDto, Timetable>()
                .ForMember(h => h.UpdatedAt, o => o.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Appointment, AppointmentDto>();
        }
    }
}
