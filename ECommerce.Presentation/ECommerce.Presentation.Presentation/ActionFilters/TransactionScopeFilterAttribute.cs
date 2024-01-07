using ECommerce.Domain.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace ECommerce.Presentation.Presentation.ActionFilters;

public class TransactionScopeFilterAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new TransactionScopeAttribute(serviceProvider.GetRequiredService<ILoggerManager>());
    }

    private class TransactionScopeAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _loggerManager;
        public TransactionScopeAttribute(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext actionExecutingContext, ActionExecutionDelegate next)
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var actionExecutedContext = await next();

            var result = actionExecutedContext.Result;
            if (result is null)
            {
                _loggerManager.LogError("ActionExecutedContext.Result is returns null!");
                return;
            }

            var type = result.GetType();
            var statusCodeProperty = type.GetProperty("StatusCode");
            if (statusCodeProperty is null)
            {
                _loggerManager.LogError("statusCodeProperty is null!");
                return;
            }

            var statusCodePropertyValue = statusCodeProperty.GetValue(result);
            if (statusCodePropertyValue is null)
            {
                _loggerManager.LogError("statusCodePropertyValue is null!");
                return;
            }

            var statusCode = (int)statusCodePropertyValue;
            if (statusCode is 200 or 201 or 204
               )
            {
                transactionScope.Complete();
            }
        }
    }
}