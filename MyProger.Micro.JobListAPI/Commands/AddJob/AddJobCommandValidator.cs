using FluentValidation;

namespace MyProger.Micro.JobListAPI.Commands.AddJob;

public class AddJobCommandValidator
    : AbstractValidator<AddJobCommand>
{
    public AddJobCommandValidator()
    {
        RuleFor(addJobCommand =>
                addJobCommand.Title).NotEqual(string.Empty)
            .WithMessage("Your title too small")
            .MaximumLength(42)
            .WithMessage("Your title too big");
        
        RuleFor(addJobCommand =>
                addJobCommand.Description).NotEqual(string.Empty)
            .WithMessage("Your description too small")
            .MaximumLength(132)
            .WithMessage("Your description too big");

        RuleFor(addJobCommand =>
                addJobCommand.Wage).NotEqual(string.Empty)
            .WithMessage("Your wage too small");

        RuleFor(addJobCommand =>
                addJobCommand.PhoneNumber).NotEqual(string.Empty)
            .WithMessage("You don't enter a phone number");
    }
}