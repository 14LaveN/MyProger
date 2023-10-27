using FluentValidation;

namespace MyProger.Micro.JobListAPI.Commands.AddLikeToJob;

public class AddLikeToJobCommandValidator
    : AbstractValidator<AddLikeToJobCommand>
{
    public AddLikeToJobCommandValidator()
    {
        RuleFor(addLikeToJobCommand =>
                addLikeToJobCommand.Id).NotEqual(Guid.Empty)
            .WithMessage("You don't enter the job id");
        
        RuleFor(addLikeToJobCommand =>
                addLikeToJobCommand.AuthorName).NotEqual(string.Empty)
            .WithMessage("You don't enter the like author");
    }
}