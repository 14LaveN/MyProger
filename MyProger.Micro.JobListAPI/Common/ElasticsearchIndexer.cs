using System.Text.Json;
using MyProger.Core.Entity.Job;
using Nest;

namespace MyProger.Mciro.JobListAPI.Common;

public class ElasticsearchIndexer
{
    private readonly IElasticClient _elasticsearchClient;

    public ElasticsearchIndexer(IElasticClient elasticsearchClient)
    {
        var settings = new ConnectionSettings(new Uri( "http://localhost:9200"))
            .DefaultIndex("jobs");
        
        _elasticsearchClient = new ElasticClient(settings);
        
        // Создание индекса и мэппинга
        _elasticsearchClient.Indices.Create("jobs", c => c
            .Map<JobEntity>(m => m.AutoMap())
        );
    }

    public async Task IndexProduct(JobEntity job)
    {
        // Добавляем документ в Elasticsearch
        await _elasticsearchClient.IndexDocumentAsync(job.ToString());
    }
}