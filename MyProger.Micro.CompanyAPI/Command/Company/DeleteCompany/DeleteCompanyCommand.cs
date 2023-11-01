using MediatR;
using MyProger.Core.Entity.Company;
using Myproger.Core.Response;

namespace MyProger.Micro.CompanyAPI.Command.Company.DeleteCompany;

public class DeleteCompanyCommand
    : IRequest<IBaseResponse<string>>
{
    public Guid CompanyId { get; set; }
}