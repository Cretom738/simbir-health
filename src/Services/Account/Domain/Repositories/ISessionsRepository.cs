using Domain.Models;

namespace Domain.Repositories
{
    public interface ISessionsRepository
    {
        Task<IList<Session>> GetListByAccountIdAsync(long accountId);

        Task<Session?> GetByIdAsync(long id);

        void Add(Session session);

        void Remove(Session session);
    }
}
