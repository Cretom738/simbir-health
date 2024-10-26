using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class TimetablesRepository : ITimetablesRepository
    {
        private readonly ApplicationDbContext _context;

        public TimetablesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Timetable>> GetListByDoctorIdAsync(long doctorId, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Timetable> query = _context.Timetables
                .Where(t => t.DoctorId == doctorId)
                .AsQueryable();

            query = from != null
                ? query.Where(t => t.From <= to)
                : query;

            query = to != null
                ? query.Where(t => from <= t.To)
                : query;

            return await query.ToListAsync();
        }

        public async Task<IList<Timetable>> GetListByHospitalIdAsync(long hospitalId, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Timetable> query = _context.Timetables
                .Where(t => t.HospitalId == hospitalId)
                .AsQueryable();

            query = from != null
                ? query.Where(t => t.From <= to)
                : query;

            query = to != null
                ? query.Where(t => from <= t.To)
                : query;

            return await query.ToListAsync();
        }

        public async Task<IList<Timetable>> GetListByHospitalRoomAsync(
            long hospitalId, string room, DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Timetable> query = _context.Timetables
                .Where(t => t.HospitalId == hospitalId)
                .Where(t => t.Room == room)
                .AsQueryable();

            query = from != null
                ? query.Where(t => t.From <= to)
                : query;

            query = to != null
                ? query.Where(t => from <= t.To)
                : query;

            return await query.ToListAsync();
        }

        public async Task<Timetable?> GetByIdAsync(long id)
        {
            return await _context.Timetables.FindAsync(id);
        }

        public async Task<Timetable?> GetByIdWithAppointmentsAsync(long id)
        {
            return await _context.Timetables
                .Include(t => t.Appointments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public void Add(Timetable timetable)
        {
            _context.Timetables.Add(timetable);
        }

        public void Remove(Timetable timetable)
        {
            _context.Timetables.Remove(timetable);
        }
    }
}
