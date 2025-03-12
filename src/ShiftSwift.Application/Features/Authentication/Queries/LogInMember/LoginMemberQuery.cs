using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;


namespace ShiftSwift.Application.Features.Authentication.Queries.LogInMember
{
    public sealed record LoginMemberQuery(
       string UserName,
       string Password) : IRequest<ErrorOr<ApiResponse<LoginMemberResult>>>;

    public sealed record LoginMemberResult(
        MemberResponse MemberResponse,
        string token);
}
