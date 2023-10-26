using System.Security.Authentication;
using DotNetLearning.Core.Exception;
using MyProger.Core.Entity;
using MyProger.Core.Enum.StatusCode;
using MyProger.Core.Response;
using MyProger.Micro.Identity.Database.Interfaces;
using MyProger.Micro.Identity.Extensions;
using MyProger.Micro.Identity.Models.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponse<AppUser>>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly IValidator<LoginCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public LoginCommandHandler(ILogger<LoginCommandHandler> logger,
        IValidator<LoginCommand> validator,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IOptions<JwtOptions> jwtOptions)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<LoginResponse<AppUser>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for login an account - {request.UserName}");

            var errors = await _validator.ValidateAsync(request, cancellationToken);

            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }
            
            var user = await _unitOfWork.AppUserRepository
                .GetByName(request.UserName);

            if (user is null)
            {
                _logger.LogWarning("User with the same name not found");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                _logger.LogWarning("The password does not meet the assessment criteria");
                throw new AuthenticationException();
            }
        
            var (refreshToken, refreshTokenExpireAt) = user.GenerateRefreshToken(_jwtOptions);
            user.RefreshToken = refreshToken;
            await _unitOfWork.AppUserRepository.SaveChanges();
        
            return new LoginResponse<AppUser>
            {
                Description = "Login account",
                StatusCode = StatusCode.Ok,
                Data = user,
                AccessToken = user.GenerateAccessToken(_jwtOptions), 
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = refreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[LoginCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}