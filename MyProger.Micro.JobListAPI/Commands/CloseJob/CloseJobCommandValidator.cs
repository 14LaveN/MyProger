using FluentValidation;

namespace MyProger.Micro.JobListAPI.Commands.CloseJob;

public class CloseJobCommandValidator
    : AbstractValidator<CloseJobCommand>
{
    public CloseJobCommandValidator()
    {
        RuleFor(closeJobCommand =>
                closeJobCommand.Id).NotEqual(Guid.Empty)
            .WithMessage("You don't enter the job id");
    }
}