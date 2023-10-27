using MediatR;
using MyProger.Core.Entity.Job;
using MyProger.Core.Entity.Likes;
using Myproger.Core.Response;

namespace MyProger.Micro.JobListAPI.Commands.AddLikeToJob;

public class AddLikeToJobCommand
    : IRequest<IBaseResponse<JobEntity>>
{
    public required Guid Id { get; set; }

    public required string AuthorName { get; set; }
    
    public static implicit operator LikeEntity(AddLikeToJobCommand addLikeToJobCommand)
    {
        return new LikeEntity
        {
            JobId = addLikeToJobCommand.Id.ToString(),
            UserName = addLikeToJobCommand.AuthorName
        };
    }
}