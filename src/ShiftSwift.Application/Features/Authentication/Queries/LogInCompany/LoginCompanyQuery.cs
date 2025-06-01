using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInCompany;

public sealed record LoginCompanyQuery(
    string UserName,
    string Password) : IRequest<ErrorOr<ApiResponse<LoginCompanyResult>>>;

public sealed record LoginCompanyResult(
    CompanyResponse CompanyResponse,
    string token);