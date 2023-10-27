using System.Text.Json;
using DotNetLearning.Core.Exception;
using FluentValidation;
using MediatR;
using MyProger.Core.Entity.Job;
using MyProger.Core.Entity.Likes;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Micro.JobListAPI.Commands.CloseJob;
using MyProger.Micro.JobListAPI.Database.Interfaces;
using MyProger.Micro.RabbitMQ.Interfaces;

namespace MyProger.Micro.JobListAPI.Commands.AddLikeToJob;

public class AddLikeToJobCommandHandler
    : IRequestHandler<AddLikeToJobCommand, IBaseResponse<JobEntity>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddLikeToJobCommand> _validator;
    private readonly ILogger<AddLikeToJobCommandHandler> _logger;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly ILikesRepository<LikeEntity> _likesRepository;
    
    public AddLikeToJobCommandHandler(IRabbitMqService rabbitMqService,
        ILogger<AddLikeToJobCommandHandler> logger,
        IValidator<AddLikeToJobCommand> validator,
        IUnitOfWork unitOfWork,
        ILikesRepository<LikeEntity> likesRepository)
    {
        _rabbitMqService = rabbitMqService;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _likesRepository = likesRepository;
    }

    public async Task<IBaseResponse<JobEntity>> Handle(AddLikeToJobCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"Request for add like to job by id");
    
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

            LikeEntity likeEntity = request;
            await _likesRepository.CreateAsync(likeEntity);
            
            job!.LikesCount += 1;
            await _unitOfWork.JobRepository.UpdateJob(job!.Id, job);
            
            var jsonProduct = JsonSerializer.Serialize(job);
            await _rabbitMqService.SendMessage(jsonProduct, "JobList");
            
            _logger.LogInformation($"Add like to job - {job.Title} {job.CreationDate}");
                
            return new BaseResponse<JobEntity>
            {
                Description = "Add like to job",
                StatusCode = StatusCode.Ok,
                Data = job,
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[AddLikeToJobCommandHandler]: {exception.Message}");
            return new BaseResponse<JobEntity>
            {
                Description = exception.Message,
                StatusCode = StatusCode.InternalServerError
            };  
        }
    } 
}