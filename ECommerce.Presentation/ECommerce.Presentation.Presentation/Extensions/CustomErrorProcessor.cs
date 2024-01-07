using ECommerce.Domain.Entities.ErrorModels;
using ECommerce.Domain.Entities.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Presentation.Extensions
{
    public static class CustomErrorProcessor
    {
        public static IActionResult Process(IResponse response)
        {
            return response switch
            {
                BadRequestResponse => new BadRequestObjectResult(new ErrorDetails
                {
                    Message = ((BadRequestResponse)response).Message,
                    StatusCode = StatusCodes.Status400BadRequest
                }),
                NotFoundResponse => new NotFoundObjectResult(new ErrorDetails
                {
                    Message = ((NotFoundResponse)response).Message,
                    StatusCode = StatusCodes.Status404NotFound
                }),
                AlreadyExistsResponse => new ConflictObjectResult(new ErrorDetails
                {
                    Message = ((AlreadyExistsResponse)response).Message,
                    StatusCode = StatusCodes.Status409Conflict
                }),
                _ => new ObjectResult(new ErrorDetails
                {
                    Message = response.Message,
                    StatusCode = StatusCodes.Status500InternalServerError
                }),
            };
        }
    }
}
