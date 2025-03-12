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

            RuleFor(x => x.MeddileName)
                .MaximumLength(50).WithMessage("MiddleName cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .MaximumLength(50).WithMessage("LastName cannot exceed 50 characters.");
        }
    }
}
