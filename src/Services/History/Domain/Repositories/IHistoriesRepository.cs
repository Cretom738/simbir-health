using Domain.Models;

namespace Domain.Repositories
{
    public interface IHistoriesRepository
    {
        Task<IList<History>> GetListByAccountIdAsync(long accountId);

        Task<IList<History>> GetListByDoctorIdAsync(long doctorId);

        Task<IList<History>> GetListByHospitalIdAsync(long hospitalId);

        Task<IList<History>> GetListByHospitalRoomAsync(long hospitalId, string room);

        Task<History?> GetByIdAsync(long id);

        void Add(History history);

        void Remove(History history);
    }
}
