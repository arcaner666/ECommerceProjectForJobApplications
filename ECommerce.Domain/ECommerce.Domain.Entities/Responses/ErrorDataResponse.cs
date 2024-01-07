namespace ECommerce.Domain.Entities.Responses;

public sealed class ErrorDataResponse<T> : DataResponse<T>
{
    public ErrorDataResponse(string message) : base(default, false, message)
    {
    }

    public ErrorDataResponse() : base(default, false)
    {
    }
}
