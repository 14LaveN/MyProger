using Microsoft.AspNetCore.Mvc;
using MyProger.Core.Helpers.Jwt;

namespace MyProger.Micro.JobListAPI.Controllers;

public class ApiBaseController : Controller
{
    protected string? Token =>
        Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
    
    [HttpGet("get-profile")]
    public string GetProfile()
    {
        var name = GetClaimByJwtToken.GetNameByToken(Token);
        
        return name;
    }
}