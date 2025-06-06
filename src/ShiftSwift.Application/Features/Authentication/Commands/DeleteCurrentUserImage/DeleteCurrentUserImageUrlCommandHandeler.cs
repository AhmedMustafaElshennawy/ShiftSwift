using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Application.services.MediaService;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.identity;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Commands.DeleteCurrentUserImage;

internal sealed class DeleteCurrentUserImageUrlCommandHandeler(
    ICurrentUserProvider currentUserProvider,
    UserManager<Account> userManager,
    IMediaService mediaService) : IRequestHandler<DeleteCurrentUserImageUrlCommand,
    ErrorOr<ApiResponse<Unit>>>
{
    public async Task<ErrorOr<ApiResponse<Unit>>> Handle(DeleteCurrentUserImageUrlCommand request,
        CancellationToken cancellationToken)
    {
        var currentUserResult = await currentUserProvider.GetCurrentUser();
        if (currentUserResult.IsError)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
        }

        var currentUser = currentUserResult.Value;

        if (!currentUser.Roles.Any(r => r is "Member" or "Company"))
        {
            return Error.Forbidden(
                code: "User.Forbidden",
                description: "Access denied. Only members or companies can upload profile pictures.");
        }

        var account = await userManager.FindByIdAsync(currentUser.UserId.ToString());
        if (account is null)
        {
            return Error.NotFound(
                code: "User.NotFound",
                description: "Account not found");
        }

        mediaService.Delete(account.ImageUrl!, MediaTypes.Image);
        await userManager.UpdateAsync(account);

        return new ApiResponse<Unit>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent,
            Message = "Profile picture deleted successfully.",
            Data = null
        };
    }
}