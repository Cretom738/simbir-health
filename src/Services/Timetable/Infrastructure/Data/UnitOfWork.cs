using Domain;
using Domain.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ITimetablesRepository Timetables { get; }

        public IAppointmentsRepository Appointments { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            ITimetablesRepository timetablesRepository,
            IAppointmentsRepository appointmentsRepository)
        {
            _context = context;

            Timetables = timetablesRepository;

            Appointments = appointmentsRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
