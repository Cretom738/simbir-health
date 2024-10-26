using Domain.Models;
using Domain.Repositories;
using Elastic.Clients.Elasticsearch;

namespace Infrastructure.Data.Repositories
{
    public class HistoriesSearchRepository : IHistoriesSearchRepository
    {
        private readonly ElasticsearchClient _client;

        public HistoriesSearchRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task CreateAsync(History history)
        {
            await _client.IndexAsync(history);
        }

        public async Task<IList<History>> GetListByAccountIdAsync(long accountId)
        {
            SearchResponse<History> response = await _client.SearchAsync<History>(qrd => qrd.Query(
                q => q.Term(
                    t => t
                        .Field(h => h.PacientId)
                        .Value(accountId)
                    )
                ));

            return response.Documents.ToList();
        }

        public async Task<History?> GetByIdAsync(long id)
        {
            GetResponse<History> response = await _client.GetAsync<History>(new Id(id));

            return response.Source;
        }

        public async Task UpdateAsync(long id, History history)
        {
            await _client.UpdateAsync<History, History>(new Id(id), urd => urd.Doc(history));
        }
    }
}
