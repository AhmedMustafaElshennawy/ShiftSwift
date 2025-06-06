using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCompanyInfo;


public sealed record GetCompanyInfoByIdQuery(string Id) : IRequest<ErrorOr<ApiResponse<CompanyInformationByIdResponse>>>;
public sealed record CompanyInformationByIdResponse(string CompanyId,
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