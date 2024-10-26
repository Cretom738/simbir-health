using Domain.Models;

namespace Domain.Repositories
{
    public interface IHospitalsRepository
    {
        Task<IList<Hospital>> GetListAsync(int from, int count);

        Task<Hospital?> GetByIdAsync(long id);

        void Add(Hospital hospital);

        void Remove(Hospital hospital);
    }
}
