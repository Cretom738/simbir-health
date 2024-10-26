using Application.Classes.Data;

namespace Application.Services
{
    public interface ISessionsService
    {
        Task<long> CreateSessionAsync(CreateSessionData data);

        Task UpdateSessionAsync(long sessionId, long tokenPairId, long oldTokenPairId);

        Task DeleteSessionAsync(long sessionId);
    }
}