﻿using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInCompany
{
    public sealed class LoginCompanyQueryHandler : IRequestHandler<LoginCompanyQuery, ErrorOr<ApiResponse<LoginCompanyResult>>>
    {
        private readonly UserManager<Account> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly SignInManager<Account> _signInManager;
        public LoginCompanyQueryHandler(UserManager<Account> userManager, ITokenGenerator tokenGenerator, SignInManager<Account> signInManager = null)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _signInManager = signInManager;
        }
        public async Task<ErrorOr<ApiResponse<LoginCompanyResult>>> Handle(LoginCompanyQuery request, CancellationToken cancellationToken)
        {

            var company = await _userManager.Users
                        .OfType<Company>() 
                        .FirstOrDefaultAsync(u => u.UserName == request.UserName,cancellationToken);
            
            if (company is null)
                return Error.NotFound(
                    code:"Account.NotFound",
                    description:"Invalid email or password.");
            
            if (company is null || !await _userManager.CheckPasswordAsync(company, request.Password))
            {
                return Error.NotFound(
                    code:"GymOwner.NotFound", 
                    description:"Login Process failed, password or UserName is wrong");
            }

            var roles = await _userManager.GetRolesAsync(company);
            if (roles == null || !roles.Any())
            {
                return Error.Forbidden(
                    code: "Account.NoRoles",
                    description: "No roles assigned to this account.");
            }

            var token = await _tokenGenerator.GenerateToken(company, roles.FirstOrDefault()!);
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
}
