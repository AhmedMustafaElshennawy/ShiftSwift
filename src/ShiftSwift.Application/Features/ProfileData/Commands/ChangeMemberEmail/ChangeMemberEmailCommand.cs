using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail
{
    public sealed record ChangeMemberEmailCommand(string MemberId,string Email)
        : IRequest<ErrorOr<ApiResponse<MemberResponse>>>;
}
