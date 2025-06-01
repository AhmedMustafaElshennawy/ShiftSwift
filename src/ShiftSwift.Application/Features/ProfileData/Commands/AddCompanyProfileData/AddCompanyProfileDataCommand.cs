using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;

public sealed record AddCompanyProfileDataCommand(
    string CompanyId,
    string FirstName,
    string LastName,
    string? Overview,
    string? Field,
    DateTime? DateOfEstablish,
    string? Country,
    string? City,
    string? Area) : IRequest<ErrorOr<ApiResponse<AddOrUpdateCompanyProfileInformationResponse>>>;


public sealed record AddOrUpdateCompanyProfileInformationResponse(string CompanyId,
    string FirstName,
    string LastName,
    string CompanyName,
    string UserName,
    string PhoneNumber,
    string Email,
    string? Overview,
    string? Field,
    DateTime? DateOfEstablish,
    string? Country,
    string? City,
    string? Area);