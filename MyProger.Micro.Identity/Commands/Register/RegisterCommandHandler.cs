using System.Security.Authentication;
using DotNetLearning.Core.Exception;
using MyProger.Core.Entity;
using MyProger.Core.Enum.StatusCode;
using MyProger.Micro.Identity.Commands.Login;
using MyProger.Micro.Identity.Database.Interfaces;
using MyProger.Micro.Identity.Extensions;
using MyProger.Micro.Identity.Models.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyProger.Core.Entity.Account;
using MyProger.Email;

namespace MyProger.Micro.Identity.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, LoginResponse<AppUser>>
{
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly IValidator<RegisterCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;
    private readonly EmailService _emailService;

    public RegisterCommandHandler(ILogger<RegisterCommandHandler> logger,
        IValidator<RegisterCommand> validator,
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IMediator mediator,
        EmailService emailService)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mediator = mediator;
        _emailService = emailService;
    }

    public async Task<LoginResponse<AppUser>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for login an account - {request.UserName} {request.Lastname}");

            var errors = await _validator.ValidateAsync(request);

            if (errors.Errors.Count is not 0)
            {
                _logger.LogWarning($"You have errors - {errors.Errors}");
                throw new ValidationException($"You have erros - {errors.Errors}");
            }
            
            var user = await _unitOfWork.AppUserRepository
                .GetByName(request.UserName);

            if (user is not null)
            {
                _logger.LogWarning("User with the same name already taken");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            user = request;
            
            var result = await _userManager.CreateAsync(user, request.RetypePassword);

            if (user.UserName != null)
            {
                await _unitOfWork.ScopesRepository.AddScope(user.UserName);
                await _unitOfWork.ScopesRepository.AddScopeToUser(user.Id, user.UserName);
            }
            
            LoginResponse<AppUser> loginResponse = new LoginResponse<AppUser>
            {
                Description = "",
                StatusCode = StatusCode.NotFound
            };

            if (result.Succeeded)
            {
                loginResponse = await _mediator.Send(new LoginCommand()
                {
                    UserName = request.UserName,
                    Password = request.RetypePassword
                });

                if (user.Email is not null && user.UserName is not null)
                    _emailService.SendEmailCustom(user.UserName, user.Email, "You authorized to MyProger");
            }
            return new LoginResponse<AppUser>
            {
                Description = "Register account",
                StatusCode = StatusCode.Ok,
                Data = user,
                AccessToken = loginResponse.AccessToken, 
                RefreshToken = loginResponse.RefreshToken,
                RefreshTokenExpireAt = loginResponse.RefreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[RegisterCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}