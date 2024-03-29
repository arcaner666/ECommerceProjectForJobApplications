﻿namespace ECommerce.Domain.Entities.Responses;

public abstract class Response : IResponse
{
    protected Response(bool success, string message) : this(success)
    {
        Message = message;
    }

    protected Response(bool success)
    {
        Success = success;
    }

    public bool Success { get; }

    public string Message { get; }
}
