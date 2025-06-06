﻿using ErrorOr;
using Microsoft.AspNetCore.Http;
using ShiftSwift.Application.services.Authentication;
using System.Security.Claims;

namespace ShiftSwift.API.Services
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
        public async Task<ErrorOr<CurrentUser>> GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
            {
                return Error.Failure(
                    code: "httpContext.Failure",
                    description: "HttpContext is not available.");
            }

            var user = httpContext.User;
            if (user is null || !user.Identity!.IsAuthenticated)
            {
                return Error.Failure(
                    code: "User.Failure",
                    description: "User is not authenticated.");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = user.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = user.FindFirst(ClaimTypes.Email)?.Value;
            var roles = user.Claims
                            .Where(c => c.Type == ClaimTypes.Role)
                            .Select(c => c.Value)
                            .ToList();

            // Validate required claims
            if (string.IsNullOrEmpty(userId))
            {
                return Error.Failure(
                    code: "userId.Failure",
                    description: "User ID claim is missing.");
            }

            if (string.IsNullOrEmpty(userName))
            {
                return Error.Failure(
                    code: "userName.Failure",
                    description: "User name claim is missing.");
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                return Error.Failure(
                    code: "userEmail.Failure",
                    description: "User email claim is missing.");
            }

            if (!roles.Any())
            {
                return Error.Failure(
                    code: "userRoles.Failure",
                    description: "User role claims are missing.");
            }

            var currentUser = new CurrentUser(
                 UserId: userId,
                 UserName: userName,
                 Email: userEmail,
                 Roles: roles);

            return currentUser;
        }
    }
}
