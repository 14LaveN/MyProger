using MyProger.Core.Enum.StatusCode;
using Myproger.Core.Response;

namespace MyProger.Core.Response;

public class BaseResponse<T> : IBaseResponse<T>
{
    public required string Description { get; set; }

    public T Data { get; set; }

    public required StatusCode StatusCode { get; set; }
}