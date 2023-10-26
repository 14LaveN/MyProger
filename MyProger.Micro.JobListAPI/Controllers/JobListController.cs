using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Entity.Job;
using MyProger.Core.Helpers.Jwt;
using Myproger.Core.Response;
using MyProger.Micro.JobListAPI.Commands.AddJob;

namespace MyProger.Micro.JobListAPI.Controllers;

[ApiController]
[Route("api/v1/job")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class JobListController : ApiBaseController
{
    private readonly IMediator _mediator;

    public JobListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IBaseResponse<JobEntity>> AddJobToCompany(AddJobCommand addJobCommand)
    {
        var name = GetProfile();
        var response = await _mediator.Send(addJobCommand);

        if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
        {
            return response;
        }

        throw new ArgumentException(response.Description);
    }
}