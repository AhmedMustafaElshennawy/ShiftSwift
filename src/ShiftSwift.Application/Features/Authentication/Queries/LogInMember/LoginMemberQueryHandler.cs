using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.identity;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Queries.LogInMember;

public sealed class LoginMemberQueryHandler(
    UserManager<Account> userManager,
    ITokenGenerator tokenGenerator,
    SignInManager<Account> singInManager)
    : IRequestHandler<LoginMemberQuery, ErrorOr<ApiResponse<LoginMemberResult>>>
{
    private readonly SignInManager<Account> _signInManager = singInManager;

    public async Task<ErrorOr<ApiResponse<LoginMemberResult>>> Handle(LoginMemberQuery request, CancellationToken cancellationToken)
    {
        var member = await userManager.Users
            .OfType<Member>()
            .FirstOrDefaultAsync(u => u.UserName == request.UserName,cancellationToken);

        if (member is null)
        {
            return Error.NotFound(
                code: "Account.NotFound",
                description: "Invalid username or password.");
        }

        var user = await userManager.FindByNameAsync(request.UserName);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Error.NotFound(
                code:"GymOwner.NotFound",
                description: "Login Process failed, password or UserName is wrong");
        }

        var roles = await userManager.GetRolesAsync(member);
        if (!roles.Any())
        {
            return Error.Forbidden(
                code: "Account.NoRoles",
                description: "No roles assigned to this account.");
        }

        var token = await tokenGenerator.GenerateToken(member, roles.FirstOrDefault()!);
        var mappedResponse = member.Adapt<LoginMemberResponse>();
        var result = new LoginMemberResult(mappedResponse, token);

        return new ApiResponse<LoginMemberResult>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Login successful",
            Data = result
        };
    }
}