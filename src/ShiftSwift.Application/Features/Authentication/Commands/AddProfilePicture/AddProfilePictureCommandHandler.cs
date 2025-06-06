using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Application.services.MediaService;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.Media;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Commands.AddProfilePicture;

public sealed class AddProfilePictureCommandHandler(
    ICurrentUserProvider currentUserProvider,
    UserManager<Account> userManager,
    IMediaService mediaService)
    : IRequestHandler<AddProfilePictureCommand, ErrorOr<ApiResponse<string>>>
{
    public async Task<ErrorOr<ApiResponse<string>>> Handle(
        AddProfilePictureCommand request,
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

        var imageUrl = await ProcessProfilePictureUpload(request.Image, account.ImageUrl);
        account.ImageUrl = imageUrl;
        await userManager.UpdateAsync(account);

        return new ApiResponse<string>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "Profile picture updated successfully.",
            Data = mediaService.GetUrl(imageUrl, MediaTypes.Image)
        };
    }

    private async Task<string?> ProcessProfilePictureUpload(IFormFile formFile, string? currentImageUrl)
    {
        var mediaFile = await ConvertToMediaFile(formFile);
        return await mediaService.UpdateAsync(mediaFile, MediaTypes.Image, currentImageUrl ?? string.Empty);
    }
    private async Task<MediaFileDto> ConvertToMediaFile(IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        return new MediaFileDto
        {
            FileName = formFile.FileName,
            Base64 = Convert.ToBase64String(fileBytes)
        };
    }
}