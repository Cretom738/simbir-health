using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Messaging
{
    public class Publisher : IPublisher
    {
        private readonly IPublishEndpoint _endpoint;

        public Publisher(IPublishEndpoint endpoint)
        {
            _endpoint = endpoint;
        }

        public async Task Publish<T>(T eventData) where T : class
        {
            await _endpoint.Publish(eventData);
        }
    }
}
