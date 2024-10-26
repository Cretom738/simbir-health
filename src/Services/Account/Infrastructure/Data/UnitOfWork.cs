using Domain;
using Domain.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IAccountsRepository Accounts { get; }

        public ISessionsRepository Sessions { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IAccountsRepository accountsRepository,
            ISessionsRepository sessionsRepository)
        {
            _context = context;

            Accounts = accountsRepository;

            Sessions = sessionsRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
