using System.Text.Json;
using DotNetLearning.Core.Exception;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Job;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.JobListAPI.Commands.AddJob;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.JobListAPI.Commands.CloseJob;

public class CloseJobCommandHandler
    : IRequestHandler<CloseJobCommand, IBaseResponse<JobEntity>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CloseJobCommand> _validator;
    private readonly ILogger<CloseJobCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public CloseJobCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<CloseJobCommandHandler> logger,
        IValidator<CloseJobCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IBaseResponse<JobEntity>> Handle(CloseJobCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for close a job by id");
    
            var errors = await _validator.ValidateAsync(request, cancellationToken);
    
            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }

            var job = await _unitOfWork.JobRepository
                .GetById(request.Id);

            if (job is null
                && job!.CompanyName == request.CompanyName)
            {
                _logger.LogWarning($"Job with the same id - {request.Id} not found");
                throw new NotFoundException(nameof(job), "Job with the same id");
            }

            job!.IsClosed = true;
            await _unitOfWork.JobRepository.UpdateJob(job!.Id, job);
            
            var jsonProduct = JsonSerializer.Serialize(job);
            await _rabbitMqService.SendMessage(jsonProduct, "JobList");
            
            _logger.LogInformation($"Close job - {job.Title} {job.CreationDate}");
                
            return new BaseResponse<JobEntity>
            {
                Description = "Close job",
                StatusCode = StatusCode.Ok,
                Data = job,
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[CloseJobCommandHandler]: {exception.Message}");
            return new BaseResponse<JobEntity>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    } 
}