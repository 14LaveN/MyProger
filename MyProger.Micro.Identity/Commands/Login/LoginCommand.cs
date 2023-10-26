using MyProger.Core.Entity;
using MyProger.Core.Response;
using MyProger.Micro.Identity.Models.Identity;
using MediatR;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Commands.Login;

public class LoginCommand
    : IRequest<LoginResponse<AppUser>>
{
    public required string UserName { get; set; } = null!;
    public required string Password { get; set; } = null!;
}