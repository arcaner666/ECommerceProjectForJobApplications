namespace ECommerce.Domain.Entities.Responses;

public interface IResponse
{
    bool Success { get; }
    string Message { get; }
}
