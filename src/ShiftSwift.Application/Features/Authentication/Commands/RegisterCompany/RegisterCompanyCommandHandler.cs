using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Commands.Register
{
    public sealed class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, ErrorOr<ApiResponse<RegisterationCompanyResult>>>
    {
        private const string DefaultCompanyRole = "Company";
        private readonly UserManager<Account> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterCompanyCommandHandler(
            UserManager<Account> userManager,
            ITokenGenerator tokenGenerator,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _roleManager = roleManager;
        }
        public async Task<ErrorOr<ApiResponse<RegisterationCompanyResult>>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) is not null)
                return Error.Forbidden("Email.Forbidden", "This email is already registered.");

            if (await _userManager.FindByNameAsync(request.UserName) is not null)
                return Error.Forbidden("UserName.Forbidden", "This username is already registered.");

            if (!await _roleManager.RoleExistsAsync(DefaultCompanyRole))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(DefaultCompanyRole));
                if (!roleResult.Succeeded)
                {
                    var errorMessages = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return Error.Failure("IdentityRole.Failure", $"Failed to create role: {errorMessages}");
                }
            }
            var company = new Company
            {
                Id = Guid.NewGuid().ToString(), 
                UserName = request.UserName, 
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(company, request.Password);
            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return Error.Unauthorized("Account.CreationFailed", $"User creation failed: {errorMessages}");
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(company, DefaultCompanyRole);
            if (!roleAssignmentResult.Succeeded)
            {
                var errorMessages = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
                return Error.Failure(
                    code: "RoleAssignment.Failure",
                    description: $"Failed to assign role: {errorMessages}");
            }
            var memberToken = await _tokenGenerator.GenerateToken(company, DefaultCompanyRole);
            var companyResponse = new CompanyResponse(company.Id,
                company.CompanyName,
                company.UserName,
                company.PhoneNumber,
                company.Email);

            var registrationCompanyResponse = new RegisterationCompanyResult(
                CompanyResponse: companyResponse,
                token: memberToken);

            return new ApiResponse<RegisterationCompanyResult>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Company registered successfully",
                Data = registrationCompanyResponse
            };
        }
    }
}