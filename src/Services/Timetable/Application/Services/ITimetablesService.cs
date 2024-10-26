using Application.Dtos;

namespace Application.Services
{
    public interface ITimetablesService
    {
        Task<TimetableDto> CreateTimetableAsync(CreateTimetableDto dto);

        Task<IEnumerable<TimetableDto>> GetTimetablesListByDoctorIdAsync(long doctorId, FilterTimetablesDto dto);

        Task<IEnumerable<TimetableDto>> GetTimetablesListByHospitalIdAsync(long hospitalId, FilterTimetablesDto dto);

        Task<IEnumerable<TimetableDto>> GetTimetablesListByHospitalRoomAsync(long hospitalId, string room, FilterTimetablesDto dto);

        Task UpdateTimetableByIdAsync(long id, UpdateTimetableDto dto);

        Task DeleteTimetablesByDoctorIdAsync(long doctorId);

        Task DeleteTimetablesByHospitalIdAsync(long hospitalId);

        Task DeleteTimetableByIdAsync(long id);
    }
}
