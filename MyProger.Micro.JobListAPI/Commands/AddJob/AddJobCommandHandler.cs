using System.Security.Authentication;
using System.Text.Json;
using DotNetLearning.Core.Exception;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Job;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.JobListAPI.Commands.AddJob;

public class AddJobCommandHandler
    : IRequestHandler<AddJobCommand, IBaseResponse<JobEntity>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddJobCommand> _validator;
    private readonly ILogger<AddJobCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public AddJobCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<AddJobCommandHandler> logger,
        IValidator<AddJobCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IBaseResponse<JobEntity>> Handle(AddJobCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for add job to company - {request.CompanyName} {request.Title}");
    
            var errors = await _validator.ValidateAsync(request, cancellationToken);
    
            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }

            var job = await _unitOfWork.JobRepository
                .GetJobByTitleAndCompany(request.CompanyName, request.Title);

            if (job is not null)
            {
                _logger.LogWarning($"Job with the same name - {request.Title} in your company already taken");
                throw new AggregateException();
            }

            job = request;
            await _unitOfWork.JobRepository.CreateJob(job);
            
            var jsonProduct = JsonSerializer.Serialize(job);
            
            await _rabbitMqService.SendMessage(jsonProduct, "JobList");
            
            _logger.LogInformation($"Add job - {job.Title} {job.CreationDate}");
                
            return new BaseResponse<JobEntity>
            {
                Description = "Add job",
                StatusCode = StatusCode.Ok,
                Data = job,
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[AddJobCommandHandler]: {exception.Message}");
            return new BaseResponse<JobEntity>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    }
}