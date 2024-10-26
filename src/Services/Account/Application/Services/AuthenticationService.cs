using Application.Classes.Constants;
using Application.Classes.Data;
using Application.Dtos;
using AutoMapper;
using Domain;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using System.Security.Claims;

namespace Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISessionsService _sessionsService;

        private readonly IJwtService _jwtService;

        private readonly IArgonService _argonService;

        private readonly IMapper _mapper;

        private readonly ClaimsPrincipal _claimsPrincipal;

        private long CurrentSessionId => long.Parse(
            _claimsPrincipal.FindFirstValue(CustomClaimTypes.SessionId)!);

        public AuthenticationService(
            IUnitOfWork unitOfWork, 
            ISessionsService sessionsService, 
            IJwtService jwtService, 
            IArgonService argonService,
            IMapper mapper,
            ClaimsPrincipal claimsPrincipal)
        {
            _unitOfWork = unitOfWork;

            _sessionsService = sessionsService;

            _jwtService = jwtService;

            _argonService = argonService;

            _mapper = mapper;

            _claimsPrincipal = claimsPrincipal;
        }

        public async Task<AuthDto> SignUpAsync(SignUpDto dto)
        {
            if (await _unitOfWork.Accounts.IsUsernameUniqueAsync(dto.Username))
            {
                throw new ConflictException("account.username_already_exists");
            }

            Account newAccount = _mapper.Map<Account>(dto);

            newAccount.Password = _argonService.Hash(dto.Password);

            _unitOfWork.Accounts.Add(newAccount);

            await _unitOfWork.SaveChangesAsync();

            long tokenPairId = new Random().NextInt64();

            long sessionId = await _sessionsService.CreateSessionAsync(new CreateSessionData(newAccount.Id, tokenPairId));

            JwtPairData pair = _jwtService.GenerateTokenPair(new GenerateJwtData(sessionId, newAccount.Id, tokenPairId, newAccount.Roles));

            return _mapper.Map<AuthDto>(pair);
        }

        public async Task<AuthDto> SignInAsync(SignInDto dto)
        {
            Account? account = await _unitOfWork.Accounts.GetByUsernameAsync(dto.Username);

            if (account == null
                || !_argonService.Compare(account.Password, dto.Password))
            {
                throw new UnauthorizedException("auth.wrong_username_or_password");
            }

            long tokenPairId = new Random().NextInt64();

            long sessionId = await _sessionsService.CreateSessionAsync(new CreateSessionData(account.Id, tokenPairId));

            JwtPairData pair = _jwtService.GenerateTokenPair(new GenerateJwtData(sessionId, account.Id, tokenPairId, account.Roles));

            return _mapper.Map<AuthDto>(pair);
        }

        public async Task SignOutAsync()
        {
            await _sessionsService.DeleteSessionAsync(CurrentSessionId);
        }

        public async Task<ValidationResultDto> ValidateAsync(ValidateDto dto)
        {
            JwtPayloadData? payload = await _jwtService.VerifyToken(dto.AccessToken, JwtTokenType.Access);

            if (payload == null)
            {
                throw new UnauthorizedException("auth.access_token_expired");
            }

            return _mapper.Map<ValidationResultDto>(payload);
        }

        public async Task<AuthDto> RefreshAsync(RefreshDto dto)
        {
            JwtPayloadData? payload = await _jwtService.VerifyToken(dto.RefreshToken, JwtTokenType.Refresh);

            if (payload == null)
            {
                throw new UnauthorizedException("auth.refresh_token_expired");
            }

            long tokenPairId = new Random().NextInt64();

            GenerateJwtData generateJwtData = new GenerateJwtData(payload.SessionId, payload.AccountId, tokenPairId, payload.Roles);

            JwtPairData pair = _jwtService.GenerateTokenPair(generateJwtData);

            await _sessionsService.UpdateSessionAsync(payload.SessionId, generateJwtData.TokenPairId, payload.TokenPairId);

            return _mapper.Map<AuthDto>(pair);
        }
    }
}
