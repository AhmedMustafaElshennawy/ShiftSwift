using FluentValidation;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData;

public sealed class AddMemberProfileDataCommandValidator : AbstractValidator<AddMemberProfileDataCommand>
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

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("DateOfBirth is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .MaximumLength(20).WithMessage("PhoneNumber cannot exceed 20 characters.")
            .Matches(@"^\+?[0-9\s\-]{7,}$").WithMessage("Invalid PhoneNumber format.");

        RuleFor(x => x.AlternativeNumber)
            .MaximumLength(20).WithMessage("AlternativeNumber cannot exceed 20 characters.")
            .Matches(@"^\+?[0-9\s\-]{7,}$").WithMessage("Invalid AlternativeNumber format.");

        RuleFor(x => x.GenderId)
            .NotEmpty().WithMessage("Gender is required.")
            .Must(g => g is 1 or 2 || g == 3)
            .WithMessage("Invalid GenderId. Allowed values: 1 (Male), 2 (Female), 3 (Other).");
    }
}