using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;
using ShiftSwift.Shared.paging;

namespace ShiftSwift.Application.Features.job.Queries.GetRandomJobs
{
    public sealed record GetRandomJobsQuery
        : PaginatedRequest, IRequest<ErrorOr<ApiResponse<PaginatedResponse<GetRandomJobsResponse>>>>
    {
       public required int JobTypeIdFilterValue { get; init; }
       public required int SalaryTypeIdFilterValue { get; init; }
    }
}
