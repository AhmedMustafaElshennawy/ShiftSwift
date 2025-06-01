using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetAllJobPosts
{
    public sealed record GetAllJobPostsForSpecificCompanyQuery
        (string CompanyId):IRequest<ErrorOr<ApiResponse<IReadOnlyList<PostedJobResponse>>>>;
}
