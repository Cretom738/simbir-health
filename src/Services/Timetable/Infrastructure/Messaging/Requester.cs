using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Messaging
{
    public class Requester : IRequester
    {
        private readonly IScopedClientFactory _clientFactory;

        public Requester(IScopedClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> GetResponse<TRequest, T>(TRequest request) 
            where TRequest : class
            where T : class
        {
            IRequestClient<TRequest> client = _clientFactory.CreateRequestClient<TRequest>();

            Response<T> response = await client.GetResponse<T>(request);

            return response.Message;
        }
    }
}
