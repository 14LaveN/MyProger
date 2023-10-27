using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Entity.Account;
using MyProger.Core.Entity.Job;
using MyProger.Core.Helpers.Jwt;

namespace MyProger.Micro.CompanyAPI.Controllers;

public class ApiBaseController : Controller
{

    protected string? Token =>
        Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    
    [HttpGet("get-name")]
    public string GetName()
    {
        var name = GetClaimByJwtToken.GetNameByToken(Token);
        return name;
    }
}