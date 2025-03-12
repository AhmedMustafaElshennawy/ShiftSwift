using FluentValidation;
using Microsoft.AspNetCore.Http;


namespace ShiftSwift.Application.Features.Authentication.Commands.AddProfilePicture
{
    public sealed class AddProfilePictureCommandValidator : AbstractValidator<AddProfilePictureCommand>
    {
        private readonly string[] _permittedExtensions = { ".jpg", ".jpeg", ".png" };
        private bool BeAValidImageFile(IFormFile file)
        {
            if (file == null) return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var result = !string.IsNullOrEmpty(extension) && _permittedExtensions.Contains(extension);
            return result;
        }
        public AddProfilePictureCommandValidator()
        {
            RuleFor(x => x.FormFile)
                .NotNull().WithMessage("Profile picture is required.")
                .Must(file => file.Length > 0).WithMessage("File cannot be empty.")
                .Must(BeAValidImageFile).WithMessage("Product image must be a valid image file (jpg, JPEG, PNG).")
                .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("File size must be less than 5MB.");
        }
    }
}
