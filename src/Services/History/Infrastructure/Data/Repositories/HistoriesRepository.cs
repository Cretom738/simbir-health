using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class HistoriesRepository : IHistoriesRepository
    {
        private readonly ApplicationDbContext _context;

        public HistoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<History>> GetListByAccountIdAsync(long accountId)
        {
            return await _context.Histories
                .Where(h => h.PacientId == accountId)
                .ToListAsync();
        }

        public async Task<IList<History>> GetListByDoctorIdAsync(long doctorId)
        {
            return await _context.Histories
                .Where(h => h.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IList<History>> GetListByHospitalIdAsync(long hospitalId)
        {
            return await _context.Histories
                .Where(h => h.HospitalId == hospitalId)
                .ToListAsync();
        }

        public async Task<IList<History>> GetListByHospitalRoomAsync(long hospitalId, string room)
        {
            return await _context.Histories
                .Where(h => h.HospitalId == hospitalId)
                .Where(h => h.Room == room)
                .ToListAsync();
        }

        public async Task<History?> GetByIdAsync(long id)
        {
            return await _context.Histories.FindAsync(id);
        }

        public void Add(History history)
        {
            _context.Histories.Add(history);
        }

        public void Remove(History history)
        {
            _context.Histories.Remove(history);
        }
    }
}
