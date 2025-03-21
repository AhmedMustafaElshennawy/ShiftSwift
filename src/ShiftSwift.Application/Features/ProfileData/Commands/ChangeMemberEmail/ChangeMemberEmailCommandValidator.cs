using FluentValidation;

namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail
{
    public sealed class ChangeMemberEmailCommandValidator : AbstractValidator<ChangeMemberEmailCommand>
    {
        public ChangeMemberEmailCommandValidator()
        {
            {
                RuleFor(x => x.MemberId)
                    .NotEmpty().WithMessage("MemberId is required.");

                RuleFor(x=>x.Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");
            }
        }
    }
}
