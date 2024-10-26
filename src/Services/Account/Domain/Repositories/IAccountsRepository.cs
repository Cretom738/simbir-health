using Domain.Models;

namespace Domain.Repositories
{
    public interface IAccountsRepository
    {
        Task<IList<Account>> GetListAsync(int from, int count);

        Task<IList<Account>> GetDoctorsListAsync(int from, int count, string? nameFilter);

        Task<Account?> GetByIdAsync(long id);

        Task<Account?> GetByUsernameAsync(string username);

        Task<Account?> GetDoctorByIdAsync(long id);

        Task<bool> IsUsernameUniqueAsync(string username);

        void Add(Account account);

        void Remove(Account account);
    }
}
