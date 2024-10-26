using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Session>> GetListByAccountIdAsync(long accountId)
        {
            return await _context.Sessions
                .Where(s => s.AccountId == accountId)
                .OrderBy(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<Session?> GetByIdAsync(long id)
        {
            return await _context.Sessions.FindAsync(id);
        }

        public void Add(Session session)
        {
            _context.Sessions.Add(session);
        }

        public void Remove(Session session)
        {
            _context.Sessions.Remove(session);
        }
    }
}
