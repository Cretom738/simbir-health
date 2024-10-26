using Domain;
using Domain.Events;
using Domain.Models;
using MassTransit;

namespace Infrastructure.Messaging.Consumers
{
    public class HospitalDeletedConsumer : IConsumer<HospitalDeleted>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HospitalDeletedConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<HospitalDeleted> context)
        {
            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByHospitalIdAsync(context.Message.HospitalId);

            foreach (Timetable timetable in timetables)
            {
                _unitOfWork.Timetables.Remove(timetable);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
