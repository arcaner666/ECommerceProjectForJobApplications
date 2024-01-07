namespace ECommerce.Domain.Entities.Responses;

public sealed class BadRequestResponse : ErrorResponse
{
    public BadRequestResponse(string message) : base(message)
    {

    }

    public BadRequestResponse()
    {

    }
}
