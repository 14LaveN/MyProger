using FluentValidation;

namespace MyProger.Micro.CompanyAPI.Command.Company.DeleteCompany;

public class DeleteCompanyCommandValidator
    : AbstractValidator<DeleteCompanyCommand>
{
    public DeleteCompanyCommandValidator()
    {
        RuleFor(deleteCompanyCommand =>
                deleteCompanyCommand.CompanyId).NotEqual(Guid.Empty)
            .WithMessage("You don't enter the company id");
    }
}