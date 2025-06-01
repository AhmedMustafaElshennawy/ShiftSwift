using System.Net;

namespace ShiftSwift.Domain.ApiResponse;

public class PaginatedResponse<TValue> : PagingResponse
{
    public PaginatedResponse() { }
    public PaginatedResponse(List<TValue> data) => Data = data;
    public PaginatedResponse(bool succeeded, List<TValue> data = default!,
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
    public new List<TValue> Data { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public static PaginatedResponse<TValue> Create(List<TValue> data, int count, int pageNumber, int pageSize)
        => new PaginatedResponse<TValue>(true, data, null!, count, HttpStatusCode.OK, pageNumber, pageSize);
}