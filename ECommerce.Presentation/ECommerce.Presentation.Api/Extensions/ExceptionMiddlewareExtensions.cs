using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ErrorModels;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ECommerce.Presentation.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature.RouteValues != null)
                {
                    var controller = contextFeature.RouteValues["controller"]?.ToString();
                    var action = contextFeature.RouteValues["action"]?.ToString();
                    var exception = contextFeature.Error;

                    logger.LogError($"Controller: {controller}, Action: {action}, ErrorMessage: {exception.Message}, InnerMessage: {exception.InnerException?.Message}");

                    var errorDetail = new ErrorDetails();

                    var options = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    };

                    errorDetail.StatusCode = context.Response.StatusCode;
                    errorDetail.Message = exception.ToString();
                    errorDetail.ControllerName = controller;
                    errorDetail.MethodName = action;

                    await context.Response.WriteAsync(
                        JsonConvert.SerializeObject(errorDetail, options)
                    );
                }
            });
        });
    }
}