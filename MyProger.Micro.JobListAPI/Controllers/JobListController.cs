using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Entity.Job;
using Myproger.Core.Response;
using MyProger.Micro.JobListAPI.Commands.AddJob;
using MyProger.Micro.JobListAPI.Commands.AddLikeToJob;
using MyProger.Micro.JobListAPI.Commands.CloseJob;
using MyProger.Micro.JobListAPI.Commands.DeleteJob;

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

    [HttpPost("add-job-to-company")]
    public async Task<IBaseResponse<JobEntity>> AddJobToCompany(AddJobCommand addJobCommand)
    {
        var userName = GetName();
        if (userName == addJobCommand.CompanyName)
        {
            var response = await _mediator.Send(addJobCommand);

            if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
            {
                return response;
            }
        }
        
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