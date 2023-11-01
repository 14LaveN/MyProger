using System.Text.Json;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Company;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.CompanyAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.CompanyAPI.Command.Company.CreateCompany;

public class CreateCompanyCommandHandler
    : IRequestHandler<CreateCompanyCommand, IBaseResponse<CompanyEntity>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCompanyCommand> _validator;
    private readonly ILogger<CreateCompanyCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public CreateCompanyCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<CreateCompanyCommandHandler> logger,
        IValidator<CreateCompanyCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IBaseResponse<CompanyEntity>> Handle(CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for create a company - {request.NameCompany} {request.Description}");
    
            var errors = await _validator.ValidateAsync(request, cancellationToken);
    
            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }

            var company = await _unitOfWork.CompanyRepository
                .GetCompanyByName(request.NameCompany);

            if (company is not null)
            {
                _logger.LogWarning($"Company with the same name - {request.NameCompany} already taken");
                throw new AggregateException();
            }

            company = request;
            await _unitOfWork.CompanyRepository
                .CreateCompany(company);
            
            var jsonProduct = JsonSerializer.Serialize(company);
            
            await _rabbitMqService.SendMessage(jsonProduct, "Company");
            
            _logger.LogInformation($"Create company - {company.NameCompany} {company.CreationDate}");
                
            return new BaseResponse<CompanyEntity>
            {
                Description = "Create company",
                StatusCode = StatusCode.Ok,
                Data = company,
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[CreateCompanyCommandHandler]: {exception.Message}");
            return new BaseResponse<CompanyEntity>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    }
}