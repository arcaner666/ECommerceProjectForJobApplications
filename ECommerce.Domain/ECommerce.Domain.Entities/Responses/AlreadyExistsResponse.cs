namespace ECommerce.Domain.Entities.Responses;

public sealed class AlreadyExistsResponse : ErrorResponse
{
    public AlreadyExistsResponse(string message) : base(message)
    {

    }

    public AlreadyExistsResponse()
    {

    }
}
