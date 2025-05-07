using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors is null || errors.Count is 0)
            {
                return Problem(
                    message: "An unknown error occurred.",
                    statusCode: HttpStatusCode.InternalServerError);
            }

            if (errors.All(error => error.Type is ErrorType.Validation)) return ValidationProblem(errors);

            var statusCode = MapErrorToStatusCode(errors.First().Type);
            return Problem(
                message: "an error occurred.",
                statusCode: statusCode,
                data: errors.Select(e => e.Description).ToList());
        }

        protected IActionResult Problem(Error error)
        {
            return Problem(
                message: error.Description,
                statusCode: MapErrorToStatusCode(error.Type));
        }

        protected IActionResult ValidationProblem(List<Error> errors)
        {
            var groupedErrors = errors
                .GroupBy(error => error.Code)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(error => error.Description).ToList());

            return Problem(
                message: "Validation failed for one or more fields.",
                statusCode: HttpStatusCode.BadRequest,
                data: groupedErrors);
        }

        private IActionResult Problem(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            object? data = null)
        {
            var response = new ApiErrorResponse
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
            return StatusCode((int)statusCode, response);
        }

        private HttpStatusCode MapErrorToStatusCode(ErrorType errorType)
        {
            var Type = errorType switch
            {
                ErrorType.Validation => HttpStatusCode.BadRequest,
                ErrorType.Unauthorized => HttpStatusCode.Unauthorized,
                ErrorType.NotFound => HttpStatusCode.NotFound,
                ErrorType.Conflict => HttpStatusCode.Conflict,
                ErrorType.Forbidden => HttpStatusCode.Forbidden,
                ErrorType.Unexpected => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };
            return Type;
        }
    }
}