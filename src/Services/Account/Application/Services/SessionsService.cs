using Application.Classes.Data;
using Application.Configurations;
using Domain;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class SessionsService : ISessionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IJwtService _jwtService;

        private readonly SessionsConfiguration _sessionsConfiguration;

        private readonly JwtConfiguration _jwtConfiguration;

        public SessionsService(
            IUnitOfWork unitOfWork,
            IJwtService jwtService,
            IOptions<SessionsConfiguration> sessionsConfiguration,
            IOptions<JwtConfiguration> jwtConfiguration)
        {
            _unitOfWork = unitOfWork;

            _jwtService = jwtService;

            _sessionsConfiguration = sessionsConfiguration.Value;

            _jwtConfiguration = jwtConfiguration.Value;
        }

        public async Task<long> CreateSessionAsync(CreateSessionData data)
        {
            IList<Session> sessions = await _unitOfWork.Sessions.GetListByAccountIdAsync(data.AccountId);

            if (sessions.Count >= _sessionsConfiguration.UserSessionsLimit)
            {
                Session oldSession = sessions[0];
                _unitOfWork.Sessions.Remove(oldSession);
                await _jwtService.BlacklistTokenAsync(oldSession.TokenPairId);
            }

            Session newSession = new Session
            {
                TokenPairId = data.TokenPairId,
                AccountId = data.AccountId,
                ExpiredAt = _jwtConfiguration.RefreshTokenExpirationDateTime
            };

            _unitOfWork.Sessions.Add(newSession);

            await _unitOfWork.SaveChangesAsync();

            return newSession.Id;
        }

        public async Task UpdateSessionAsync(long sessionId, long tokenPairId, long oldTokenPairId)
        {
            Session? session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);

            if (session == null)
            {
                throw new UnauthorizedException("auth.session_expired_or_invalid_refresh_token");
            }

            session.TokenPairId = tokenPairId;

            session.UpdatedAt = DateTime.UtcNow;

            await Task.WhenAll(
                _unitOfWork.SaveChangesAsync(),
                _jwtService.BlacklistTokenAsync(oldTokenPairId)
            );
        }

        public async Task DeleteSessionAsync(long sessionId)
        {
            Session? session = await _unitOfWork.Sessions.GetByIdAsync(sessionId);

            if (session == null)
            {
                throw new UnauthorizedException("auth.not_authorized");
            }

            _unitOfWork.Sessions.Remove(session);

            await Task.WhenAll(
                _unitOfWork.SaveChangesAsync(),
                _jwtService.BlacklistTokenAsync(session.TokenPairId)
            );
        }
    }
}
