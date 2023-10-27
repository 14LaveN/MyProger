using MediatR;
using MyProger.Core.Entity.Company;
using Myproger.Core.Response;

namespace MyProger.Micro.CompanyAPI.Command.Company.CreateCompany;

public class CreateCompanyCommand
    : IRequest<IBaseResponse<CompanyEntity>>
{
    public required string NameCompany { get; set; } = null!;
    
    public required string Description { get; set; } = null!;
    
    public static implicit operator CompanyEntity(CreateCompanyCommand createCompanyCommand)
    {
        return new CompanyEntity()
        {
            CreationDate = DateTime.Now,
            NameCompany = createCompanyCommand.NameCompany,
            Description = createCompanyCommand.Description,
            JobsCount = 0,
            ReviewsCount = 0
        };
    }
}