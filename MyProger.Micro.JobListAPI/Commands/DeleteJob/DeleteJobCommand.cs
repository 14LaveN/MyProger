using MediatR;
using Myproger.Core.Response;

namespace MyProger.Micro.JobListAPI.Commands.DeleteJob;

public class DeleteJobCommand
    : IRequest<IBaseResponse<string>>
{
    public required Guid Id { get; set; }
    
    public required string CompanyName { get; set; }
}