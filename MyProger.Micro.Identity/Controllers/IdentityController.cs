using MyProger.Micro.Identity.Extensions;
using MyProger.Micro.Identity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyProger.Core.Entity;
using MyProger.Micro.Identity.Commands.Login;
using MyProger.Micro.Identity.Commands.Register;
using MyProger.Micro.Identity.Database;
using MediatR;

namespace MyProger.Micro.Identity.Controllers;

[ApiController]
[Route("api/v1/identity")]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class IdentityController : ApiBaseController
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _context;

    public IdentityController(AppDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var response = await _mediator.Send(request);
        if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
        {
            return Ok(new
            {
                description = response.Description,
                accessToken = response.AccessToken
            });
        }
        return BadRequest(new { descritpion = response.Description});
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        var response = await _mediator.Send(request);
        if (response.StatusCode == Core.Enum.StatusCode.StatusCode.Ok)
        {
            return Ok(new
            {
                description = response.Description,
                accessToken = response.AccessToken
            });
        }
        return BadRequest(new { descritpion = response.Description });
    }

    [HttpGet("get-profile")]
    [Authorize]
    public string? GetProfile()
    {
        var name = User.Claims.FirstOrDefault(x=> x.Type == "name")?.Value;
        return name;
    }
}