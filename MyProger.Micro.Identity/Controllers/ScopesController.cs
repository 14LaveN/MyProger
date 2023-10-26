using MyProger.Core.Entity;
using MyProger.Micro.Identity.Database;
using MyProger.Micro.Identity.Database.Interfaces;
using MyProger.Micro.Identity.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Controllers;

[ApiController]
[Route("scopes")]
public class ScopesController : ApiBaseController
{
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public ScopesController(AppDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    [Authorize(Scope.ScopesAllRead)]
    public async Task<IActionResult> GetAll()
    {
        var scopes = await _context.Scopes.Select(x => x.Name).ToListAsync();
        var scopeString = string.Join(' ', scopes);
        
        return Ok(new {scopes, scopeString});
    }

    [HttpGet("my")]
    [Authorize()]
    public async Task<IActionResult> GetMy()
    {
        var user = await _context.Users
            .Include(x => x.Scopes)
            .FirstOrDefaultAsync(x => x.Id == CurrentUser.Id);
        var scopes = string.Join(' ', user?.Scopes?.Select(x => x.Name) ?? Array.Empty<string>());
        
        return Ok(new {scopes});
    }

    [HttpPost]
    [Authorize(Scope.ScopesWrite)]
    public async Task<IActionResult> Create([FromBody] string scopeName)
    {
        await _unitOfWork.ScopesRepository.AddScope(new ScopeEntity { Name = scopeName });

        return Ok();
    }

    [HttpPost("{userId:long}")]
    [Authorize(Scope.ScopesWrite)]
    public async Task<IActionResult> AddToUser(long userId, string scopeName)
    {
        var scope = await _unitOfWork.ScopesRepository.AddScopeToUser(userId,scopeName);
        
        if (scope == null) return BadRequest("Scope not found");

        return Ok();
    }

    [HttpDelete("{userId:long}")]
    [Authorize(Scope.ScopesWrite)]
    public async Task<IActionResult> RemoveFromUser(long userId, string scopeName)
    {
        var scope = _unitOfWork.ScopesRepository.RemoveScopeFromUser(userId,scopeName);
        
        if (scope == null) return BadRequest("Scope not found");

        return Ok();
    }
}