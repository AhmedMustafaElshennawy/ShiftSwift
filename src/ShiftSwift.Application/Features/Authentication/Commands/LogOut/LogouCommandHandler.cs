using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using Microsoft.AspNetCore.Http;

namespace ShiftSwift.Application.Features.Authentication.Commands.LogOut
{

    namespace ShiftSwift.Application.Features.Authentication.Commands.Logout
    {
        public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, ErrorOr<ApiResponse<string>>>
        {
            private readonly SignInManager<Account> _signInManager;
            private readonly IHttpContextAccessor _httpContextAccessor;
            public LogoutCommandHandler(SignInManager<Account> signInManager,IHttpContextAccessor httpContextAccessor)
            {
                _signInManager = signInManager;
                _httpContextAccessor = httpContextAccessor;
            }
            public async Task<ErrorOr<ApiResponse<string>>> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null || !httpContext.User.Identity?.IsAuthenticated == true)
                {
                    return Error.Unauthorized("User is not authenticated.");
                }

                // Sign out the user from authentication session
                await _signInManager.SignOutAsync();

                return new ApiResponse<string>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Logged out successfully.",
                    Data = "User has been logged out."
                };
            }
        }
    }
}
