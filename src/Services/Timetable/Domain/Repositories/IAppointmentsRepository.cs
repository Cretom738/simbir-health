using Domain.Models;

namespace Domain.Repositories
{
    public interface IAppointmentsRepository
    {
        Task<IList<Appointment>> GetListByTimetableIdAsync(long timetableId);

        Task<Appointment?> GetByIdAsync(long id);

        void Add(Appointment appointment);

        void Remove(Appointment appointment);
    }
}
