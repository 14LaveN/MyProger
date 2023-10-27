using MediatR;
using MyProger.Core.Entity.Job;
using Myproger.Core.Response;

namespace MyProger.Micro.JobListAPI.Commands.UpdateJob;

public class UpdateJobCommand
    : IRequest<IBaseResponse<JobEntity>>
{
    public required Guid Id { get; set; }
    public required string CompanyName { get; set; } = null!;

    public required string Description { get; set; } = null!;

    public required string Title { get; set; } = null!;

    public required string Wage { get; set; } = null!; //! Decimal to string

    public required string PhoneNumber { get; set; } = null!;
    
    public static implicit operator JobEntity(UpdateJobCommand updateJobCommand)
    {
        return new JobEntity()
        {
            Id = updateJobCommand.Id,
            CompanyName = updateJobCommand.CompanyName,
            Title = updateJobCommand.Title,
            PhoneNumber = updateJobCommand.PhoneNumber,
            Wage = updateJobCommand.Wage,
            CreationDate = DateTime.Now,
            LikesCount = 0,
            IsClosed = false,
            ViewsCount = 1,
            Description = updateJobCommand.Description
        };
    }
}