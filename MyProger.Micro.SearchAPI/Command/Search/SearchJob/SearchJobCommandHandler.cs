
using System.Security.Authentication;
using DotNetLearning.Core.Exception;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyProger.Core.Entity.Account;
using MyProger.Core.Entity.Job;
using MyProger.Core.Entity.Mail;
using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;
using MyProger.Core.Response;
using MyProger.Mciro.SearchAPI.Service;

namespace MyProger.Mciro.SearchAPI.Command.Search.SearchJob;

public class SearchJobCommandHandler
    : IRequestHandler<SearchJobCommand, IBaseResponse<List<JobEntity>>>
{
    private readonly ILogger<SearchJobCommandHandler> _logger;
    private readonly IValidator<SearchJobCommand> _validator;
    private readonly ISearchService _searchService;

    public SearchJobCommandHandler(ILogger<SearchJobCommandHandler> logger,
        IValidator<SearchJobCommand> validator, 
        ISearchService searchService)
    {
        _logger = logger;
        _validator = validator;
        _searchService = searchService;
    }

    public async Task<IBaseResponse<List<JobEntity>>> Handle(SearchJobCommand request,
        CancellationToken cancellationToken)
    
    {
        try
        {
            _logger.LogInformation($"Request for search - {request.Query} {DateTime.Now}");

            var errors = await _validator.ValidateAsync(request, cancellationToken);

            if (errors.Errors.Count is not 0)
            {
                _logger.LogWarning($"You have errors - {errors.Errors}");
                throw new ValidationException($"You have erros - {errors.Errors}");
            }

            if (request.Query is null)
            {
                _logger.LogWarning("Query with the same description not found");
                throw new NotFoundException(nameof(request.Query), "Query with the same description");
            }

            var response = await _searchService.SearchProductsAsync(request.Query);
            
            _logger.LogInformation($"Found by request - {response} {DateTime.Now}");
            
            return new BaseResponse<List<JobEntity>>
            {
                Description = "Found by request",
                StatusCode = StatusCode.Ok,
                Data = response
            };
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, $"[SearchJobCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}