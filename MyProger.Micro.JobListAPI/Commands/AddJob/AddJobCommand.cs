using MediatR;
using MyProger.Core.Entity.Job;
using Myproger.Core.Response;

namespace MyProger.Micro.JobListAPI.Commands.AddJob;

public class AddJobCommand
    : IRequest<IBaseResponse<JobEntity>>
{
    public required string CompanyName { get; set; } = null!;

    public required string Description { get; set; } = null!;

    public required string Title { get; set; } = null!;

    public required string Wage { get; set; } = null!; //! Decimal to string

    public required string PhoneNumber { get; set; } = null!;
    
    public static implicit operator JobEntity(AddJobCommand addJobCommand)
    {
        return new JobEntity()
        {
            CompanyName = addJobCommand.CompanyName,
            Title = addJobCommand.Title,
            PhoneNumber = addJobCommand.PhoneNumber,
            Wage = addJobCommand.Wage,
            CreationDate = DateTime.Now,
            LikesCount = 0,
            IsClosed = false,
            ViewsCount = 1,
            Description = addJobCommand.Description
        };
    }
}