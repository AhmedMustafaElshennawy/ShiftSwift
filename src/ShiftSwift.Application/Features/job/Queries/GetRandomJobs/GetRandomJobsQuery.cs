using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.paging;

namespace ShiftSwift.Application.Features.job.Queries.GetRandomJobs
{
    public sealed record GetRandomJobsQuery()
        :PaginatedRequest,IRequest<ErrorOr<PaginatedResponse<GetRandomJobsResponse>>>;
}
