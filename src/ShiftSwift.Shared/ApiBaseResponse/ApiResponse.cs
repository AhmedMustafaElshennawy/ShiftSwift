using System.Net;

namespace ShiftSwift.Shared.ApiBaseResponse
{
    public class ApiResponse<TEntity>
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
