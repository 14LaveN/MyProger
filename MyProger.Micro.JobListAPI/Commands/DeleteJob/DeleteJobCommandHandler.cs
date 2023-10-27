using System.Text.Json;
using DotNetLearning.Core.Exception;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Job;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.JobListAPI.Commands.CloseJob;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.JobListAPI.Commands.DeleteJob;

public class DeleteJobCommandHandler
    : IRequestHandler<DeleteJobCommand, IBaseResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteJobCommand> _validator;
    private readonly ILogger<DeleteJobCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;

    public DeleteJobCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<DeleteJobCommandHandler> logger,
        IValidator<DeleteJobCommand> validator,
        IUnitOfWork unitOfWork)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IBaseResponse<string>> Handle(DeleteJobCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for delete a job by id");
    
            var errors = await _validator.ValidateAsync(request, cancellationToken);
    
            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }

            var job = await _unitOfWork.JobRepository
                .GetById(request.Id);

            if (job is null)
            {
                _logger.LogWarning($"Job with the same id - {request.Id} not found");
                throw new NotFoundException(nameof(job), "Job with the same id");
            }

            await _unitOfWork.JobRepository.DeleteJob(job.Id);
            
            var jsonProduct = JsonSerializer.Serialize(job);
            await _rabbitMqService.SendMessage(jsonProduct, "JobList");
            
            _logger.LogInformation($"Delete job - {job.Title} {job.CreationDate}");
                
            return new BaseResponse<string>
            {
                Description = "Delete job",
                StatusCode = StatusCode.Ok,
                Data = job.Description
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[DeleteJobCommandHandler]: {exception.Message}");
            return new BaseResponse<string>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    } 
}