using FluentValidation;

namespace MyProger.Micro.JobListAPI.Commands.UpdateJob;

public class UpdateJobCommandValidator
    : AbstractValidator<UpdateJobCommand>
{
    public UpdateJobCommandValidator()
    {
        RuleFor(updateJobCommand =>
                updateJobCommand.Title).NotEqual(string.Empty)
            .WithMessage("Your title too small")
            .MaximumLength(42)
            .WithMessage("Your title too big");
        
        RuleFor(updateJobCommand =>
                updateJobCommand.Description).NotEqual(string.Empty)
            .WithMessage("Your description too small")
            .MaximumLength(132)
            .WithMessage("Your description too big");

        RuleFor(updateJobCommand =>
                updateJobCommand.Wage).NotEqual(string.Empty)
            .WithMessage("Your wage too small");

        RuleFor(updateJobCommand =>
                updateJobCommand.Id).NotEqual(Guid.Empty)
            .WithMessage("You don't enter the job id");
        
        RuleFor(updateJobCommand =>
                updateJobCommand.PhoneNumber).NotEqual(string.Empty)
            .WithMessage("You don't enter a phone number");
    }
}