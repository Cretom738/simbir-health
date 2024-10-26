using Domain;
using Domain.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IHistoriesRepository Histories { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IHistoriesRepository histories)
        {
            _context = context;

            Histories = histories;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
