using System.Net;


namespace ShiftSwift.Shared.paging
{
    public class PaginatedResponse<TResponse> : PagingResponse
    {
        public PaginatedResponse() { }
        public PaginatedResponse(List<TResponse> data) => Data = data;
        public PaginatedResponse(bool succeeded, List<TResponse> data = default!,
                                string message = null!,
                                int count = 0,
                                HttpStatusCode httpStatusCode = HttpStatusCode.OK,
                                int pageNumber = 1,
                                int pageSize = 10)
        {
            Data = data;
            CurrentPage = pageNumber;
            StatusCode = httpStatusCode;
            IsSuccess = succeeded;
            Message = message;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }
        public new List<TResponse> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public static PaginatedResponse<TResponse> Create(List<TResponse> data, int count, int pageNumber, int pageSize)
            => new PaginatedResponse<TResponse>(true, data, null!, count, HttpStatusCode.OK, pageNumber, pageSize);
    }
}
