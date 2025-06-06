using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserInformation;

public sealed record GetCurrentUserInformationQuery():IRequest<ErrorOr<ApiResponse<object>>>;


public sealed record CurrentCompanyResponse(string CompanyId,
    string FirstName,
    string LastName,
    string UserName,
    string PhoneNumber,
    string Email,
    string? Overview,
    string? Field,
    DateTime? DateOfEstablish,
    string? Country,
    string? City,
    string? Area);

public sealed record CurrentMemberResponse(
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