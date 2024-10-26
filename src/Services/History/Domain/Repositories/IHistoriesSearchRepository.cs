using Domain.Models;

namespace Domain.Repositories
{
    public interface IHistoriesSearchRepository
    {
        Task CreateAsync(History history);

        Task<IList<History>> GetListByAccountIdAsync(long accountId);

        Task<History?> GetByIdAsync(long id);

        Task UpdateAsync(long id, History history);
    }
}
