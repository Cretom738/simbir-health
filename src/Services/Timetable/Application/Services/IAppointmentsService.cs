using Application.Dtos;

namespace Application.Services
{
    public interface IAppointmentsService
    {
        Task<AppointmentDto> CreateAppointmentAsync(long timetableId, CreateAppointmentDto dto);

        Task<IEnumerable<DateTime>> GetAvaliableAppointmentsListByAsync(long timetableId);

        Task DeleteAppointmentByIdAsync(long id);
    }
}
