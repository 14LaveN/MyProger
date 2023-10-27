using MediatR;
using MyProger.Core.Entity.Job;
using Myproger.Core.Response;

namespace MyProger.Micro.JobListAPI.Commands.CloseJob;

public class CloseJobCommand
    : IRequest<IBaseResponse<JobEntity>>
{
    public required Guid Id { get; set; }

    public required string CompanyName { get; set; }
}