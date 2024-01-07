namespace ECommerce.Domain.Entities.ErrorModels;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? ControllerName { get; set; }
    public string? MethodName { get; set; }
}