using Domain;
using Domain.Enums;
using Domain.Events;
using Domain.Models;
using MassTransit;

namespace Infrastructure.Messaging.Consumers
{
    public class AccountDeletedConsumer : IConsumer<AccountDeleted>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountDeletedConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<AccountDeleted> context)
        {
            AccountDeleted eventData = context.Message;

            if (!eventData.Roles.Contains(Role.Doctor))
            {
                return;
            }

            IList<Timetable> timetables = await _unitOfWork.Timetables.GetListByDoctorIdAsync(eventData.AccountId);

            foreach (Timetable timetable in timetables)
            {
                _unitOfWork.Timetables.Remove(timetable);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
