using FluentValidation;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData
{
    public sealed class AddMemberProfileDataCommandValidator:AbstractValidator<AddMemberProfileDataCommand>
    {
        public AddMemberProfileDataCommandValidator()
        {
            RuleFor(x => x.MemberId)
                .NotEmpty().WithMessage("MemberId is required.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .MaximumLength(50).WithMessage("FirstName cannot exceed 50 characters.");

            RuleFor(x => x.Location)
                .MaximumLength(50).WithMessage("Location cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .MaximumLength(50).WithMessage("LastName cannot exceed 50 characters.");

            RuleFor(x => x.GenderId)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == 1 || g == 2 || g == 3)
                .WithMessage("Invalid GenderId. Allowed values: 1 (Male), 2 (Female), 3 (Other).");
        }
    }
}
