using MyProger.Core.Entity;
using MyProger.Micro.Identity.Models.Identity;
using MediatR;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Commands.Register;

public class RegisterCommand : IRequest<LoginResponse<AppUser>>
{
    public required string Email { get; set; } = null!;

    public required string UserName { get; set; } = null!;

    public required string RetypePassword { get; set; } = null!;
    
    public required string? Firstname { get; set; }
    
    public required string? Lastname { get; set; }
    
    public required string? Patronymic { get; set; }
    
    public required DateTime? BirthDate { get; set; }
    
    public static implicit operator AppUser(RegisterCommand registerCommand)
    {
        return new AppUser()
        {
            Email = registerCommand.Email,
            Firstname = registerCommand.Firstname,
            Lastname = registerCommand.Lastname,
            Patronymic = registerCommand.Patronymic,
            BirthDate = registerCommand.BirthDate,
            UserName = registerCommand.UserName
        };
    }
}