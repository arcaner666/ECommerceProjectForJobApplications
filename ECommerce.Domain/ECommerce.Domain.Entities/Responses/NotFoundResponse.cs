namespace ECommerce.Domain.Entities.Responses;

public sealed class NotFoundResponse : ErrorResponse
{
    public NotFoundResponse(string message) : base(message)
    {

    }

    public NotFoundResponse()
    {

    }
}
