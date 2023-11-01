using MyProger.Core.Entity.Job;
using Nest;

namespace MyProger.Mciro.SearchAPI.Service;

public class SearchService : IDisposable
{
    private readonly IElasticClient _client;

    public SearchService()
    {
        var settings = new ConnectionSettings(new Uri( "http://localhost:7120"))
            .DefaultIndex("jobs");

        //?? var jobs = dbContext.Products.ToList(); // Получите данные из базы данных
        //??  var indexResponse = _client.IndexMany(jobs,"jobs");
        
        _client = new ElasticClient(settings);
        
        // Создание индекса и мэппинга
        _client.Indices.Create("jobs", c => c
            .Map<JobEntity>(m => m.AutoMap())
        );
    }

    public async Task IndexProductAsync(JobEntity product)
    {
        await _client.IndexDocumentAsync(product);
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