using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.Registermamber;

public record RegisterMemberCommand(
    string Email,
    string UserName,
    string Password,
    string PhoneNumber) : IRequest<ErrorOr<ApiResponse<RegisterationMemberResult>>>;

public sealed record RegisterationMemberResult(
    RegisterMemberResponse MemberResponse,
    string Token);

public sealed record RegisterMemberResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int GenderId,
    string Location);