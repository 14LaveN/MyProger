using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MyProger.Core.Entity.Job;
using MyProger.Core.Models;
using Myproger.Core.Response;
using MyProger.Mciro.JobListAPI.Common;
using MyProger.Micro.JobListAPI.Commands.AddJob;
using MyProger.Micro.JobListAPI.Commands.AddLikeToJob;
using MyProger.Micro.JobListAPI.Commands.CloseJob;
using MyProger.Micro.JobListAPI.Commands.DeleteJob;
using MyProger.Micro.JobListAPI.Monitoring;
using StackExchange.Redis;

namespace MyProger.Micro.JobListAPI.Controllers;

[ApiController]
[Route("api/v1/job")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class JobListController : ApiBaseController
{
    private readonly IDistributedCache _cache;
    private readonly IMediator _mediator;
    //private readonly IMetricsFactory _metricsFactory;
    private readonly ElasticsearchIndexer _elasticsearchIndexer;

    public JobListController(IMediator mediator,
        IDistributedCache cache,
        //IMetricsFactory metricsFactory,
        ElasticsearchIndexer elasticsearchIndexer)
    {
        _mediator = mediator;
        _cache = cache;
        //_metricsFactory = metricsFactory;
        _elasticsearchIndexer = elasticsearchIndexer;
    }

    [HttpPost("add-job-to-company")]
    public async Task<IBaseResponse<JobEntity>> AddJobToCompany(AddJobCommand addJobCommand)
    {
        //double numbers = 0;
        //var counter = _metricsFactory.CreateCounter(new CounterConfiguration("loxs", "dfgfd"));
        //counter.Increment(ref numbers);
        
        //var userName = GetName();
        //if (userName == addJobCommand.CompanyName)
        //{
            var response = await _mediator.Send(addJobCommand);
            //await _cache.SetAsync($"{addJobCommand.Title}", Encoding.UTF8.GetBytes(response.ToString() ?? string.Empty));
            if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
            {
                await _elasticsearchIndexer.IndexProduct(response.Data);
                return response;
            }
        //}
        
        throw new ArgumentException("It didn't work, create a job");
    }
    
    [HttpPost("add-like-to-job")]
    public async Task<IBaseResponse<JobEntity>> AddLikeToJob(AddLikeToJobCommand addLikeToJobCommand)
    {
        var userName = GetName();
        if (userName == addLikeToJobCommand.AuthorName)
        {
            var response = await _mediator.Send(addLikeToJobCommand);

            if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
            {
                return response;
            }
        }
        
        throw new ArgumentException("It didn't work, create a job");
    }
    
    [HttpPost("close-job-by-id")]
    public async Task<IBaseResponse<JobEntity>> CloseJobById(CloseJobCommand closeJobCommand)
    {
        string name = GetName();
        closeJobCommand.CompanyName = name;
        
        if (closeJobCommand.CompanyName == name)
        {
            var response = await _mediator.Send(closeJobCommand);

            if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
            {
                return response;
            }
        }

        throw new ArgumentException("It didn't work, close a job");
    }
    
    [HttpDelete("delete-job-by-id")]
    public async Task<IBaseResponse<string>> DeleteJobById(DeleteJobCommand deleteJobCommand)
    {
        string name = GetName();
        deleteJobCommand.CompanyName = name;
        
        if (deleteJobCommand.CompanyName == name)
        {
            var response = await _mediator.Send(deleteJobCommand);

            if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
            {
                return response;
            }
        }
        
        throw new ArgumentException("It didn't work, delete a job");
    }
}