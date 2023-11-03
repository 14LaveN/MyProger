using System.Diagnostics;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Entity.Job;
using MyProger.Mciro.SearchAPI.Command.Search.SearchJob;
using MyProger.Mciro.SearchAPI.Service;
using Prometheus;

namespace MyProger.Mciro.SearchAPI.Controllers.V1;

[ApiController]
[Route("api/v1/search")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class SearchController : Controller
{
    private static readonly Counter RequestCounter = Metrics.CreateCounter("yourapp_requests_total", "Total number of requests.");
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-job")]
    public async Task<List<JobEntity>> GetJob([FromQuery] string request)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await _mediator.Send(new SearchJobCommand
        {
            Query = request
        });
        
        stopwatch.Stop();
        
        RequestCounter.Inc();
        
        Metrics.CreateHistogram("yourapp_request_duration_seconds", "Request duration in seconds.")
            .Observe(stopwatch.Elapsed.TotalMilliseconds);

        if (response.Data.Count is not 0 
            && response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
        {
            return response.Data;
        }

        throw new ArgumentException();
    }
}