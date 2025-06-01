using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;

public sealed record RegisterCompanyCommand(
    string Email,
    string UserName,
    string Password,
    string PhoneNumber) : IRequest<ErrorOr<ApiResponse<RegisterationCompanyResult>>>;
public sealed record RegisterationCompanyResult(CompanyResponse CompanyResponse, string Token);