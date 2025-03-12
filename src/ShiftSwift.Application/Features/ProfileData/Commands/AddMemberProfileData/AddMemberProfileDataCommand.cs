using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData
{
    public sealed record AddMemberProfileDataCommand(string MemberId,
        string FirstName,
        string MeddileName,
        string LastName) : IRequest<ErrorOr<ApiResponse<MemberResponse>>>;
}
