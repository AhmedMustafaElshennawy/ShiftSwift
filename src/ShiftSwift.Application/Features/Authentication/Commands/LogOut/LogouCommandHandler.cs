using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using ShiftSwift.Domain.identity;
using Microsoft.AspNetCore.Http;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Commands.LogOut;

public sealed class LogoutCommandHandler(
    SignInManager<Account> signInManager,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<LogoutCommand, ErrorOr<ApiResponse<string>>>
{
    public async Task<ErrorOr<ApiResponse<string>>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null || !httpContext.User.Identity?.IsAuthenticated == true)
        {
            return Error.Unauthorized("User is not authenticated.");
        }

        // Sign out the user from authentication session
        await signInManager.SignOutAsync();

        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Logged out successfully.",
            Data = "User has been logged out."
        };
    }
}