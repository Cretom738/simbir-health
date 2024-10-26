using Application.Dtos;

namespace Application.Services
{
    public interface IHistoriesService
    {
        Task<HistoryDto> CreateHistoryAsync(CreateHistoryDto dto);

        Task<IEnumerable<HistoryDto>> GetHistoriesListByAccountIdAsync(long accountId);

        Task<HistoryDto> GetHistoryByIdAsync(long id);

        Task UpdateHistoryByIdAsync(long id, UpdateHistoryDto dto);
    }
}
