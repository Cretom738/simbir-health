using Application.Dtos;

namespace Application.Services
{
    public interface IHospitalsService
    {
        Task<HospitalDto> CreateHospitalAsync(CreateHospitalDto dto);

        Task<IEnumerable<HospitalDto>> GetHospitalsListAsync(FilterHospitalsDto dto);

        Task<HospitalDto> GetHospitalByIdAsync(long id);

        Task<IEnumerable<string>> GetHospitalRoomsListAsync(long id);

        Task UpdateHospitalByIdAsync(long id, UpdateHospitalDto dto);

        Task DeleteHospitalByIdAsync(long id);
    }
}
