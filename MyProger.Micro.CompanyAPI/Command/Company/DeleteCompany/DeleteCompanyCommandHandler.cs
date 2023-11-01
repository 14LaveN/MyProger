using System.Text.Json;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Company;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.CompanyAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.CompanyAPI.Command.Company.DeleteCompany;

public class DeleteCompanyCommandHandler
    : IRequestHandler<DeleteCompanyCommand, IBaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteCompanyCommand> _validator;
    private readonly ILogger<DeleteCompanyCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public DeleteCompanyCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<DeleteCompanyCommandHandler> logger,
        IValidator<DeleteCompanyCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IBaseResponse<string>> Handle(DeleteCompanyCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for delete a company by id- {request.CompanyId} {DateTime.Now}");
    
            var errors = await _validator.ValidateAsync(request, cancellationToken);
    
            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }

            var company = await _unitOfWork.CompanyRepository
                .GetById(request.CompanyId);

            if (company is null)
            {
                _logger.LogWarning($"Company with the same id - {request.CompanyId} not found");
                throw new AggregateException();
            }
            
            await _unitOfWork.CompanyRepository
                .DeleteCompany(company.Id);
            
            var jsonProduct = JsonSerializer.Serialize(company);
            
            await _rabbitMqService.SendMessage(jsonProduct, "Company");
            
            _logger.LogInformation($"Delete company - {company.NameCompany} {company.CreationDate}");
                
            return new BaseResponse<string>
            {
                Description = "Delete company",
                StatusCode = StatusCode.Ok,
                Data = $"Delete Company by id - {request.CompanyId}"
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[DeleteCompanyCommandHandler]: {exception.Message}");
            return new BaseResponse<string>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    }
}