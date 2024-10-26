using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Account>> GetListAsync(int from, int count)
        {
            return await _context.Accounts
                .Skip(from)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IList<Account>> GetDoctorsListAsync(int from, int count, string? nameFilter)
        {
            IQueryable<Account> query = _context.Accounts
                .Where(a => a.Roles.Contains(Role.Doctor))
                .AsQueryable();

            nameFilter = nameFilter?.ToLower();

            query = nameFilter != null
                ? query.Where(a => a.FirstName.ToLower().Contains(nameFilter) 
                    || a.LastName.ToLower().Contains(nameFilter))
                : query;

            return await query
                .Skip(from)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(long id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account?> GetByUsernameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Username == username);
        }

        public async Task<Account?> GetDoctorByIdAsync(long id)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.Roles.Contains(Role.Doctor));
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return await _context.Accounts.AnyAsync(a => a.Username == username);
        }

        public void Add(Account account)
        {
            _context.Accounts.Add(account);
        }

        public void Remove(Account account)
        {
            _context.Accounts.Remove(account);
        }
    }
}
