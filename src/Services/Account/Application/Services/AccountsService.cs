using Application.Dtos;
using AutoMapper;
using Domain;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using System.Security.Claims;

namespace Application.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IArgonService _argonService;

        private readonly IMapper _mapper;

        private readonly ClaimsPrincipal _claimsPrincipal;

        private readonly IPublisher _publisher;

        private long CurrentAccountId => long.Parse(
            _claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public AccountsService(
            IUnitOfWork unitOfWork, 
            IArgonService argonService, 
            IMapper mapper, 
            ClaimsPrincipal claimsPrincipal,
            IPublisher publisher)
        {
            _unitOfWork = unitOfWork;

            _argonService = argonService;

            _mapper = mapper;

            _claimsPrincipal = claimsPrincipal;

            _publisher = publisher;
        }

        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto)
        {
            if (await _unitOfWork.Accounts.IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException("account.username_already_exists");
            }

            Account newAccount = _mapper.Map<Account>(dto);

            newAccount.Password = _argonService.Hash(dto.Password);

            _unitOfWork.Accounts.Add(newAccount);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(newAccount);
        }

        public async Task<IEnumerable<AccountDto>> GetAccountsListAsync(FilterAccountsDto dto)
        {
            IEnumerable<Account> accounts = await _unitOfWork.Accounts.GetListAsync(dto.From, dto.Count);

            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<IEnumerable<AccountDto>> GetDoctorsListAsync(FilterDoctorAccountsDto dto)
        {
            IEnumerable<Account> accounts = await _unitOfWork.Accounts.GetDoctorsListAsync(dto.From, dto.Count, dto.NameFilter);

            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<AccountDto> GetDoctorByIdAsync(long id)
        {
            Account? account = await _unitOfWork.Accounts.GetDoctorByIdAsync(id);

            if (account == null)
            {
                throw new NotFoundException("account.not_found");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> GetCurrentAccountAsync()
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(CurrentAccountId);

            if (account == null)
            {
                throw new UnauthorizedException("auth.not_authorized");
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task UpdateAccountByIdAsync(long id, UpdateAccountDto dto)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                throw new NotFoundException("account.not_found");
            }

            if (account.Username != dto.Username
                && await _unitOfWork.Accounts.IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException("account.username_already_exists");
            }

            _mapper.Map(dto, account);

            account.Password = _argonService.Hash(dto.Password);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCurrentAccountAsync(UpdateCurrentAccountDto dto)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(CurrentAccountId);

            if (account == null)
            {
                throw new UnauthorizedException("auth.not_authorized");
            }

            _mapper.Map(dto, account);

            account.Password = _argonService.Hash(dto.Password);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAccountByIdAsync(long id)
        {
            if (CurrentAccountId == id)
            {
                throw new BadRequestException("account.cannot_delete_current_account");
            }

            Account? account = await _unitOfWork.Accounts.GetByIdAsync(id);

            if (account == null)
            {
                throw new NotFoundException("account.not_found");
            }

            _unitOfWork.Accounts.Remove(account);

            await _unitOfWork.SaveChangesAsync();

            await _publisher.Publish(new AccountDeleted(id, account.Roles));
        }
    }
}
