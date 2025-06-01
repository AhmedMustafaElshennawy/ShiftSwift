using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetShortlistedMembers
{
    public sealed record GetShortlistedMembersQuery(Guid JobId)
        : IRequest<ErrorOr<ApiResponse<List<ApplyApplicantResponse>>>>;
}
