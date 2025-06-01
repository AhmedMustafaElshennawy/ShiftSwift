using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInCompany;

public sealed class LoginCompanyQueryHandler(
    UserManager<Account> userManager,
    ITokenGenerator tokenGenerator,
    SignInManager<Account> signInManager)
    : IRequestHandler<LoginCompanyQuery, ErrorOr<ApiResponse<LoginCompanyResult>>>
{
    private readonly SignInManager<Account> _signInManager = signInManager;

    public async Task<ErrorOr<ApiResponse<LoginCompanyResult>>> Handle(LoginCompanyQuery request, CancellationToken cancellationToken)
    {

        var company = await userManager.Users
            .OfType<Company>() 
            .FirstOrDefaultAsync(u => u.UserName == request.UserName,cancellationToken);
            
        if (company is null)
            return Error.NotFound(
                code:"Account.NotFound",
                description:"Invalid email or password.");
            
        if (company is null || !await userManager.CheckPasswordAsync(company, request.Password))
        {
            return Error.NotFound(
                code:"GymOwner.NotFound", 
                description:"Login Process failed, password or UserName is wrong");
        }

        var roles = await userManager.GetRolesAsync(company);
        if (roles == null || !roles.Any())
        {
            return Error.Forbidden(
                code: "Account.NoRoles",
                description: "No roles assigned to this account.");
        }

        var token = await tokenGenerator.GenerateToken(company, roles.FirstOrDefault()!);
        var companyResponse = new CompanyResponse(company.Id,
            company.CompanyName,
            company.UserName!,
            company.PhoneNumber!,
            company.Email!);

        var loginCompanyResponse = new LoginCompanyResult(
            CompanyResponse: companyResponse,
            token: token);

        return new ApiResponse<LoginCompanyResult>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Login successful",
            Data = loginCompanyResponse
        };
    }
}