using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Entity.Job;
using MyProger.Mciro.SearchAPI.Command.Search.SearchJob;
using MyProger.Mciro.SearchAPI.Service;

namespace MyProger.Mciro.SearchAPI.Controllers.V1;

[ApiController]
[Route("api/v1/search")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class SearchController : Controller
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-job")]
    public async Task<List<JobEntity>> GetJob([FromQuery] string request)
    {
        var response = await _mediator.Send(new SearchJobCommand
        {
            Query = request
        });
        
        if (response.Data.Count is not 0 
            && response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
        {
            return response.Data;
        }

        throw new ArgumentException();
    }
}