using Application.Dtos;

namespace Application.Services
{
    public interface IAccountsService
    {
        Task<AccountDto> CreateAccountAsync(CreateAccountDto dto);

        Task<IEnumerable<AccountDto>> GetAccountsListAsync(FilterAccountsDto dto);

        Task<IEnumerable<AccountDto>> GetDoctorsListAsync(FilterDoctorAccountsDto dto);

        Task<AccountDto> GetCurrentAccountAsync();

        Task<AccountDto> GetDoctorByIdAsync(long id);

        Task UpdateAccountByIdAsync(long id, UpdateAccountDto dto);

        Task UpdateCurrentAccountAsync(UpdateCurrentAccountDto dto);

        Task DeleteAccountByIdAsync(long id);
    }
}
