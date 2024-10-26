using Domain;
using Domain.Events;
using Domain.Models;
using MassTransit;

namespace Infrastructure.Messaging.Consumers
{
    public class HospitalUpdatedConsumer : IConsumer<HospitalUpdated>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HospitalUpdatedConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<HospitalUpdated> context)
        {
            HospitalUpdated eventData = context.Message;

            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByHospitalIdAsync(eventData.HospitalId);

            foreach (Timetable timetable in timetables)
            {
                if (!eventData.Rooms.Contains(timetable.Room))
                {
                    _unitOfWork.Timetables.Remove(timetable);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
