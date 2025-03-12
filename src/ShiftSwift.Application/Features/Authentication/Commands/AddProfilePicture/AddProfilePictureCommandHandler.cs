using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Commands.AddProfilePicture
{
    public sealed class AddProfilePictureCommandHandler : IRequestHandler<AddProfilePictureCommand, ErrorOr<ApiResponse<string>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<Account> _userManager;
        public AddProfilePictureCommandHandler(ICurrentUserProvider currentUserProvider,
            IWebHostEnvironment webHostEnvironment,
            UserManager<Account> userManager)
        {
            _currentUserProvider = currentUserProvider;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public async Task<ErrorOr<ApiResponse<string>>> Handle(AddProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
            }

            var currentUser = currentUserResult.Value;
            string userTypeFolder;
            if (currentUser.Roles.Contains("Member"))
            {
                userTypeFolder = "Members";
            }
            else if (currentUser.Roles.Contains("Company"))
            {
                userTypeFolder = "Companies";
            }
            else
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Access denied. Only members or companies can upload profile pictures.");
            }

            var account = await _userManager.FindByIdAsync(currentUser.UserId.ToString());
            if (account is null) return Error.NotFound("Account not found");

            if (!string.IsNullOrEmpty(account.ImageUrl))
            {
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, account.ImageUrl);
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }

            string imageFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.FormFile.FileName)}";
            string imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", userTypeFolder, "Pictures");

            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            string imagePath = Path.Combine(imagesFolder, imageFileName);
            using (var stream = new FileStream(imagePath, FileMode.Create))
                await request.FormFile.CopyToAsync(stream);

            var pictureUrl = Path.Combine("Images", userTypeFolder, "Pictures", imageFileName).Replace("\\", "/");
            string imageUrl = account.ImageUrl = pictureUrl;
            await _userManager.UpdateAsync(account);

            return new ApiResponse<string>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Message = "Picture added successfully.",
                Data = imageUrl
            };
        }
    }
}
