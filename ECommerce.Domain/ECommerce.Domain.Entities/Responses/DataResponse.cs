namespace ECommerce.Domain.Entities.Responses;

public abstract class DataResponse<T> : Response, IDataResponse<T>
{
    protected DataResponse(T data, bool success, string message) : base(success, message)
    {
        Data = data;
    }

    protected DataResponse(T data, bool success) : base(success)
    {
        Data = data;
    }

    public T Data { get; }
}
