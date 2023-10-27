using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Entity.Account;
using MyProger.Core.Entity.Job;
using MyProger.Core.Helpers.Jwt;
using MyProger.Micro.Identity.Database.Interfaces;

namespace MyProger.Micro.JobListAPI.Controllers;

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