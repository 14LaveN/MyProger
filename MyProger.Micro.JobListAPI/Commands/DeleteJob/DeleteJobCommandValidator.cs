using FluentValidation;

namespace MyProger.Micro.JobListAPI.Commands.DeleteJob;

public class DeleteJobCommandValidator
    : AbstractValidator<DeleteJobCommand>
{
    public DeleteJobCommandValidator()
    {
        RuleFor(deleteJobCommand =>
                deleteJobCommand.Id).NotEqual(Guid.Empty)
            .WithMessage("You don't enter the job id");
    }
}