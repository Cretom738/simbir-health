using Domain;
using Domain.Events;
using Domain.Models;
using MassTransit;

namespace Infrastructure.Messaging.Consumers
{
    public class CheckHospitalExistanceConsumer : IConsumer<CheckHospitalExistance>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckHospitalExistanceConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<CheckHospitalExistance> context)
        {
            Hospital? hospital = await _unitOfWork.Hospitals.GetByIdAsync(context.Message.HospitalId);

            await context.RespondAsync(new HospitalExistanceResult(hospital?.Rooms ?? []));
        }
    }
}
