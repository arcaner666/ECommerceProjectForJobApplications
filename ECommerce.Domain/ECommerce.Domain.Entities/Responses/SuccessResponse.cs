namespace ECommerce.Domain.Entities.Responses;

public sealed class SuccessResponse : Response
{
    public SuccessResponse(string message) : base(true, message)
    {

    }

    public SuccessResponse() : base(true)
    {

    }
}
