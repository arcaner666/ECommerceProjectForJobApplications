namespace ECommerce.Domain.Entities.Responses;

public interface IDataResponse<T> : IResponse
{
    T Data { get; }
}
