using System.Net;
using System.Text.Json;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.API.MiddleWare;

public sealed class GlobalExceptionHandlerMiddleWare(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }

    }
    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {

        var response = new ApiErrorResponse
        {
            IsSuccess = false,
            Message = ex.Message,
            StatusCode = HttpStatusCode.InternalServerError,
            Data = null
        };

        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var exceptionResult = JsonSerializer.Serialize(response, jsonOptions);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(exceptionResult);
    }
}