using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetJobPostById
{
    public sealed record GetJobPostByIdQuery(Guid JobId)
        : IRequest<ErrorOr<ApiResponse<JobInfoResponse>>>;
}
