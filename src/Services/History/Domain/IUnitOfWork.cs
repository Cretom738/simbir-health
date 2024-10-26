using Domain.Repositories;

namespace Domain
{
    public interface IUnitOfWork
    {
        public IHistoriesRepository Histories { get; }

        Task SaveChangesAsync();
    }
}
