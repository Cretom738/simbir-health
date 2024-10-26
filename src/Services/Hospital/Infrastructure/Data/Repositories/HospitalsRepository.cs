using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class HospitalsRepository : IHospitalsRepository
    {
        private readonly ApplicationDbContext _context;

        public HospitalsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Hospital>> GetListAsync(int from, int count)
        {
            return await _context.Hospitals
                .Skip(from)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Hospital?> GetByIdAsync(long id)
        {
            return await _context.Hospitals.FindAsync(id);
        }

        public void Add(Hospital hospital)
        {
            _context.Hospitals.Add(hospital);
        }

        public void Remove(Hospital hospital)
        {
            _context.Hospitals.Remove(hospital);
        }
    }
}
