using System.Net;

namespace ShiftSwift.Domain.ApiResponse;

public class ApiResponse<TResult>
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
}