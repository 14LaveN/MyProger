using System.Text.Json;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Job;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.JobListAPI.Commands.UpdateJob;

public class UpdateJobCommandHandler
    : IRequestHandler<UpdateJobCommand, IBaseResponse<JobEntity>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateJobCommand> _validator;
    private readonly ILogger<UpdateJobCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public UpdateJobCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<UpdateJobCommandHandler> logger,
        IValidator<UpdateJobCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IBaseResponse<JobEntity>> Handle(UpdateJobCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for update job to company - {request.CompanyName} {request.Title}");
    
            var errors = await _validator.ValidateAsync(request, cancellationToken);
    
            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }

            var job = await _unitOfWork.JobRepository
                .GetById(request.Id);

            if (job is null)
            {
                _logger.LogWarning($"Job with the same id - {request.Id} in your company not found");
                throw new AggregateException();
            }

            job = request;
            await _unitOfWork.JobRepository.UpdateJob(job.Id, job);
            
            var jsonProduct = JsonSerializer.Serialize(job);
            
            await _rabbitMqService.SendMessage(jsonProduct, "JobList");
            
            _logger.LogInformation($"Update job - {job.Title} {job.CreationDate}");
                
            return new BaseResponse<JobEntity>
            {
                Description = "Update job",
                StatusCode = StatusCode.Ok,
                Data = job,
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[UpdateJobCommandHandler]: {exception.Message}");
            return new BaseResponse<JobEntity>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    }
}