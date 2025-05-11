using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetShortlistedMembers
{
    public sealed record GetShortlistedMembersQuery(Guid JobId)
        : IRequest<ErrorOr<ApiResponse<List<ApplyApplicantResponse>>>>;
}
