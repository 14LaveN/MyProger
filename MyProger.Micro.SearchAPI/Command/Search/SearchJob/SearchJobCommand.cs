using MediatR;
using MyProger.Core.Entity.Job;
using Myproger.Core.Response;

namespace MyProger.Mciro.SearchAPI.Command.Search.SearchJob;

public class SearchJobCommand
    : IRequest<IBaseResponse<List<JobEntity>>>
{
    public required string Query { get; set; }
}