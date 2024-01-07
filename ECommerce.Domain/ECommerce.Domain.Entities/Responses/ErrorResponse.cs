namespace ECommerce.Domain.Entities.Responses;

public class ErrorResponse : Response
{
    public ErrorResponse(string message) : base(false, message)
    {

    }

    public ErrorResponse() : base(false)
    {

    }
}
