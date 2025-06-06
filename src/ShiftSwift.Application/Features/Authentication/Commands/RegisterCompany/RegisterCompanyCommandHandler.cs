using System.Net;
using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;

public sealed class RegisterCompanyCommandHandler(
    UserManager<Account> userManager,
    ITokenGenerator tokenGenerator,
    RoleManager<IdentityRole> roleManager)
    : IRequestHandler<RegisterCompanyCommand, ErrorOr<ApiResponse<RegisterationCompanyResult>>>
{
    private const string DefaultCompanyRole = "Company";

    public async Task<ErrorOr<ApiResponse<RegisterationCompanyResult>>> Handle(RegisterCompanyCommand request,
        CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
            return Error.Forbidden(
                code:"Email.Forbidden", 
                description:"This email is already registered.");

        if (await userManager.FindByNameAsync(request.UserName) is not null)
            return Error.Forbidden(
                code:"UserName.Forbidden",
                description:"This username is already registered.");

        if (!await roleManager.RoleExistsAsync(DefaultCompanyRole))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(DefaultCompanyRole));
            if (!roleResult.Succeeded)
            {
                var errorMessages = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Error.Failure(
                    code:"IdentityRole.Failure", 
                    description:$"Failed to create role: {errorMessages}");
            }
        }

        var company = new Company
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.UserName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };

        var result = await userManager.CreateAsync(company, request.Password);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return Error.Unauthorized(
                code:"Account.CreationFailed", 
                description:$"User creation failed: {errorMessages}");
        }

        var roleAssignmentResult = await userManager.AddToRoleAsync(company, DefaultCompanyRole);
        if (!roleAssignmentResult.Succeeded)
        {
            var errorMessages = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
            return Error.Failure(
                code: "RoleAssignment.Failure",
                description: $"Failed to assign role: {errorMessages}");
        }

        var memberToken = await tokenGenerator.GenerateToken(company, DefaultCompanyRole);

        var companyResponse = company.Adapt<CompanyResponse>();
        var registrationCompanyResponse = new RegisterationCompanyResult(
            CompanyResponse: companyResponse,
            Token: memberToken);

        return new ApiResponse<RegisterationCompanyResult>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Company registered successfully",
            Data = registrationCompanyResponse
        };
    }
}