using Domain;
using Domain.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IHospitalsRepository Hospitals { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IHospitalsRepository hospitalsRepository)
        {
            _context = context;

            Hospitals = hospitalsRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
