namespace ECommerce.Domain.Entities.Responses.Extensions;

public static class ResponseExtensions
{
    public static SuccessDataResponse<T> Convert<T>(this IResponse response)
    {
        return (SuccessDataResponse<T>)response;
    }
}