using AutoMapper.Internal;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ErrorModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Presentation.Presentation.ActionFilters;

public class ValidationFilterAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => true;
    private readonly Type _validatorType;

    public ValidationFilterAttribute(object validatorTypeObject)
    {
        var validatorType = (Type)validatorTypeObject;
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
            throw new Exception("Invalid validation class!");

        _validatorType = validatorType;
    }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new ValidationAttribute(
            serviceProvider.GetRequiredService<ILoggerManager>(),
            _validatorType
        );
    }

    private class ValidationAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _loggerManager;
        private readonly Type _validatorType;

        public ValidationAttribute(ILoggerManager loggerManager, Type validatorType)
        {
            _loggerManager = loggerManager;
            _validatorType = validatorType;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext actionExecutingContext,
            ActionExecutionDelegate next)
        {
            var action = actionExecutingContext.RouteData.Values["action"] as string;
            var controller = actionExecutingContext.RouteData.Values["controller"] as string;

            var arguments = actionExecutingContext.ActionArguments;

            var keys = arguments.Keys;

            var errorDetails = new ErrorDetails
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity,
                ControllerName = controller,
                MethodName = action
            };

            var dtoKey = keys.SingleOrDefault(k => k.Contains("Dto"));
            if (dtoKey is null)
            {
                _loggerManager.LogError(
                    $"Controller: {controller}, Action: {action}, Attribute: ValidationFilterAttribute, Error: Data Transformation Object is null!");

                errorDetails.Message = "Data Transformation Object is null!";
                actionExecutingContext.Result =
                    new UnprocessableEntityObjectResult(errorDetails);
                return;
            }

            if (!arguments.TryGetValue(dtoKey, out var dto))
            {
                _loggerManager.LogError(
                    $"Controller: {controller}, Action: {action}, Attribute: ValidationFilterAttribute, Error: Can not get value from method arguments!");

                errorDetails.Message = "Can not get value from method arguments!";
                actionExecutingContext.Result =
                    new UnprocessableEntityObjectResult(errorDetails);
                return;
            }

            if (dto is null)
            {
                _loggerManager.LogError(
                    $"Controller: {controller}, Action: {action}, Attribute: ValidationFilterAttribute, Error: {dtoKey} is null!");

                errorDetails.Message =
                    $"{dtoKey[0].ToString().ToUpper()}{dtoKey[1..]}IsNull";
                actionExecutingContext.Result =
                    new UnprocessableEntityObjectResult(errorDetails);
                return;
            }

            if (dto.GetType().IsListType())
            {
                var entities = new List<object>((IEnumerable<object>)dto);

                var validator = (IValidator)Activator.CreateInstance(_validatorType);


                foreach (var entity in entities)
                {
                    var context = new ValidationContext<object>(entity);

                    if (validator != null)
                    {
                        var result = await validator.ValidateAsync(context);
                        if (!result.IsValid)
                        {
                            var validationFailures = result.Errors;
                            errorDetails.StatusCode = StatusCodes.Status422UnprocessableEntity;
                            errorDetails.Message = validationFailures.FirstOrDefault()?.ErrorMessage;

                            actionExecutingContext.Result =
                                new UnprocessableEntityObjectResult(errorDetails);

                            return;
                        }
                    }
                }
            }
            else
            {
                var validator = (IValidator)Activator.CreateInstance(_validatorType);

                var context = new ValidationContext<object>(dto);

                if (validator != null)
                {
                    var result = await validator.ValidateAsync(context);
                    if (!result.IsValid)
                    {
                        var validationFailures = result.Errors;
                        errorDetails.StatusCode = StatusCodes.Status422UnprocessableEntity;
                        errorDetails.Message = validationFailures.FirstOrDefault()?.ErrorMessage;

                        actionExecutingContext.Result =
                            new UnprocessableEntityObjectResult(errorDetails);

                        return;
                    }
                }
            }

            await next();
        }
    }
}