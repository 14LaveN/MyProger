using MyProger.Core.Enum.StatusCode;

namespace Myproger.Core.Response;

public interface IBaseResponse<T>
{
    public StatusCode StatusCode { get; set; }

    public string Description { get; set; }
    
    public T Data { get; set; }
}