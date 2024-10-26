using Domain.Repositories;

namespace Domain
{
    public interface IUnitOfWork
    {
        IHospitalsRepository Hospitals { get; }

        Task SaveChangesAsync();
    }
}
