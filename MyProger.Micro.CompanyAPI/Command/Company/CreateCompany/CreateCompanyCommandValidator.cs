using FluentValidation;

namespace MyProger.Micro.CompanyAPI.Command.Company.CreateCompany;

public class CreateCompanyCommandValidator 
    : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(createCompanyCommand =>
                createCompanyCommand.NameCompany).NotEqual(string.Empty)
            .WithMessage("You don't enter a company name")
            .MaximumLength(46)
            .WithMessage("Your company name is too long");
        
        RuleFor(createCompanyCommand =>
                createCompanyCommand.Description).NotEqual(string.Empty)
            .WithMessage("You don't enter a description")
            .MaximumLength(456)
            .WithMessage("Your description is too long");
    }
}