using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData;

public sealed record AddMemberProfileDataCommand(string MemberId,
    string FirstName,
    string LastName,
    string Location,
    string PhoneNumber,
    string AlternativeNumber,
    int GenderId,
    DateTime DateOfBirth) : IRequest<ErrorOr<ApiResponse<AddMemberProfileDataResponse>>>;

public sealed record AddMemberProfileDataResponse(
    string MemberId,
    string FirstName,
    string LastName,
    string UserName,
    string PhoneNumber,
    string AlternativeNumber,
    string Email,
    int GenderId,
    string Location,
    DateTime DateOfBirth);