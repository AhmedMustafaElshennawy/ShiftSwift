using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInMember
{
    public sealed class LoginMemberQueryHandler : IRequestHandler<LoginMemberQuery, ErrorOr<ApiResponse<LoginMemberResult>>>
    {
        private readonly UserManager<Account> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly SignInManager<Account> _signInManager;
        public LoginMemberQueryHandler(UserManager<Account> userManager, ITokenGenerator tokenGenerator, SignInManager<Account> singInManager)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _signInManager = singInManager;
        }
        public async Task<ErrorOr<ApiResponse<LoginMemberResult>>> Handle(LoginMemberQuery request, CancellationToken cancellationToken)
        {
            var member = await _userManager.Users
                .OfType<Member>()
                .FirstOrDefaultAsync(u => u.UserName == request.UserName,cancellationToken);

            if (member is null)
            {
                return Error.NotFound(
                    code: "Account.NotFound",
                    description: "Invalid username or password.");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return Error.NotFound(
                    code:"GymOwner.NotFound",
                    description: "Login Process failed, password or UserName is wrong");
            }

            var roles = await _userManager.GetRolesAsync(member);
            if (roles == null || !roles.Any())
            {
                return Error.Forbidden(
                    code: "Account.NoRoles",
                    description: "No roles assigned to this account.");
            }

            var token = await _tokenGenerator.GenerateToken(member, roles.FirstOrDefault()!);
            var MemberResponse = new MemberResponse(member.Id,
                member.FullName,
                member.UserName!,
                member.PhoneNumber!,
                member.Email!,
                member.GenderId.Value,
                member.Location);

            var loginCompanyResponse = new LoginMemberResult(
                MemberResponse: MemberResponse,
                token: token);

            return new ApiResponse<LoginMemberResult>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Login successful",
                Data = loginCompanyResponse
            };
        }
    }
}