using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData
{
    public sealed record AddMemberProfileDataCommand(string MemberId,
        string FirstName,
        string Location,
        string LastName,
        int GenderId) : IRequest<ErrorOr<ApiResponse<MemberResponse>>>;
}
