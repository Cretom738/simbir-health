using Domain;
using Domain.Events;
using Domain.Models;
using MassTransit;

namespace Infrastructure.Messaging.Consumers
{
    public class CheckAccountExistanceConsumer : IConsumer<CheckAccountExistance>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckAccountExistanceConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<CheckAccountExistance> context)
        {
            Account? account = await _unitOfWork.Accounts.GetByIdAsync(context.Message.AccountId);

            await context.RespondAsync(new AccountExistanceResult(account?.Roles ?? []));
        }
    }
}
