using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.Authentication.Queries.LogInMember;

public sealed record LoginMemberQuery(
    string UserName,
    string Password) : IRequest<ErrorOr<ApiResponse<LoginMemberResult>>>;

public sealed record LoginMemberResult(
    LoginMemberResponse MemberResponse,
    string Token);

public sealed record LoginMemberResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int GenderId,
    string Location);