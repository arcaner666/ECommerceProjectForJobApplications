using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Service.Contracts.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Presentation.Presentation.ActionFilters;

public class AuthorizationFilterAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new AuthorizationAttribute(serviceProvider.GetRequiredService<ILoggerManager>());
    }

    private class AuthorizationAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _loggerManager;

        public AuthorizationAttribute(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext actionExecutingContext,
            ActionExecutionDelegate next)
        {
            var action = actionExecutingContext.RouteData.Values["action"] as string;
            var controller = actionExecutingContext.RouteData.Values["controller"] as string;

            var personId = actionExecutingContext.HttpContext.User.PersonId();
            var role = actionExecutingContext.HttpContext.User.Role();

            var incomingRoleClaims = actionExecutingContext.HttpContext.User.RoleClaims();

            if (!incomingRoleClaims.Contains($"{controller}-{action}"))
            {
                _loggerManager.LogError(
                    $"Controller: {controller}, Action: {action}, Attribute: {nameof(AuthorizationFilterAttribute)}, Error: {Messages.AuthorizationDenied}");
                actionExecutingContext.Result =
                    new UnauthorizedObjectResult(new ErrorResponse(Messages.AuthorizationDenied));
                return;
            }

            var actionExecutedContext = await next();
        }
    }
}