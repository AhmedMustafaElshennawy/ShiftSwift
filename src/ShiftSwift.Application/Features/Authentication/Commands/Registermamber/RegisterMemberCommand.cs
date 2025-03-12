using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.Registermamber
{
    public record RegisterMemberCommand(
        string Email,
        string UserName,
        string Password,
        string PhoneNumber) : IRequest<ErrorOr<ApiResponse<RegisterationMemberResult>>>;

    public sealed record RegisterationMemberResult(
        MemberResponse MemberResponse,
        string token);
}
