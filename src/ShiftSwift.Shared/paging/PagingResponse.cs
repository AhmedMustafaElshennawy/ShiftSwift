using System.Net;

namespace ShiftSwift.Shared.paging
{
    public class PagingResponse
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
