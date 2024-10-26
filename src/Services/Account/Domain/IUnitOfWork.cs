using Domain.Repositories;

namespace Domain
{
    public interface IUnitOfWork
    {
        IAccountsRepository Accounts { get; }

        ISessionsRepository Sessions { get; }

        Task SaveChangesAsync();
    }
}
