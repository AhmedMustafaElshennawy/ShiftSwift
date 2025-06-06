using ErrorOr;
using MediatR;
using ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInCompany;

public sealed record LoginCompanyQuery(
    string UserName,
    string Password) : IRequest<ErrorOr<ApiResponse<LoginCompanyResult>>>;

public sealed record LoginCompanyResult(
    LoginCompanyResponse CompanyResponse,
    string Token);

public sealed record LoginCompanyResponse(string CompanyId,
    string UserName,
    string PhoneNumber,
    string Email);