using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Appointment>> GetListByTimetableIdAsync(long timetableId)
        {
            return await _context.Appointments
                .Where(a => a.TimetableId == timetableId)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(long id)
        {
            return await _context.Appointments.FindAsync(id);
        }

        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }

        public void Remove(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }
    }
}
