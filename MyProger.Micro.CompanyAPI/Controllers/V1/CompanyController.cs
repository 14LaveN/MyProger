using System.Security.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProger.Core.Entity.Company;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Micro.CompanyAPI.Command.Company.CreateCompany;

namespace MyProger.Micro.CompanyAPI.Controllers.V1;

[ApiController]
[Route("api/v1/job")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class CompanyController : ApiBaseController
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IBaseResponse<CompanyEntity>> CreateCompany(CreateCompanyCommand createCompanyCommand)
    {
        var name = GetName();
        
        if (!name.IsNullOrEmpty())
        {
            var response = await _mediator.Send(createCompanyCommand);
        
            if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
            {
                return response;
            }
        }

        throw new AuthenticationException("You don't authorized");
    }
}