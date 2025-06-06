using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;

public sealed record RegisterCompanyCommand(
    string Email,
    string UserName,
    string Password,
    string PhoneNumber) : IRequest<ErrorOr<ApiResponse<RegisterationCompanyResult>>>;

public sealed record CompanyResponse(string CompanyId,
    string UserName,
    string PhoneNumber,
    string Email);

public sealed record RegisterationCompanyResult(CompanyResponse CompanyResponse, string Token);