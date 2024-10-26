using Domain.Models;

namespace Domain.Repositories
{
    public interface ITimetablesRepository
    {
        Task<IList<Timetable>> GetListByHospitalIdAsync(long hospitalId, DateTime? from = null, DateTime? to = null);

        Task<IList<Timetable>> GetListByDoctorIdAsync(long doctorId, DateTime? from = null, DateTime? to = null);

        Task<IList<Timetable>> GetListByHospitalRoomAsync(long hospitalId, string room, DateTime? from = null, DateTime? to = null);

        Task<Timetable?> GetByIdAsync(long id);

        Task<Timetable?> GetByIdWithAppointmentsAsync(long id);

        void Add(Timetable timetable);

        void Remove(Timetable timetable);
    }
}
