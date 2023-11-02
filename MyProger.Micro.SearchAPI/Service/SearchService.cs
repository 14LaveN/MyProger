using MyProger.Core.Entity.Job;
using Nest;

namespace MyProger.Mciro.SearchAPI.Service;

public class SearchService : IDisposable, ISearchService
{
    private readonly IElasticClient _client;

    public SearchService()
    {
        var settings = new ConnectionSettings(new Uri( "http://localhost:9200"))
            .DefaultIndex("jobs");
        
        _client = new ElasticClient(settings); 
        
        // Создание индекса и мэппинга
        _client.Indices.Create("jobs", c => c
            .Map<JobEntity>(m => m.AutoMap())
        );
    }
    
    public async Task<JobEntity> UpdateJob(Guid id, JobEntity jobEntity)
    {
        await _client.UpdateAsync<JobEntity>(id,
            u => u.Doc(jobEntity));

        return jobEntity;
    }

    public async Task DeleteJob(Guid id)
    {
        await _client.DeleteAsync<JobEntity>(id);
    }
    
    public async Task<List<JobEntity>> SearchProductsAsync(string query)
    {
        var searchResponse = await _client.SearchAsync<JobEntity>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(p => p.Title)
                        .Field(p => p.Description)
                    )
                    .Query(query)
                )
            )
        );

        return searchResponse.Documents.ToList();
    }

    public void Dispose()
    {
        _client?.ClosePointInTime();
    }
}