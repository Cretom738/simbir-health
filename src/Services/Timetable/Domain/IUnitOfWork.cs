using Domain.Repositories;

namespace Domain
{
    public interface IUnitOfWork
    {
        ITimetablesRepository Timetables { get; }

        IAppointmentsRepository Appointments { get; }

        Task SaveChangesAsync();
    }
}
