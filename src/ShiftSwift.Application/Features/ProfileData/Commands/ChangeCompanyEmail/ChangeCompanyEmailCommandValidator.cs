using FluentValidation;

namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeCompanyEmail
{
    public sealed class ChangeCompanyEmailCommandValidator:AbstractValidator<ChangeCompanyEmailCommand>
    {
        public ChangeCompanyEmailCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                  .NotEmpty().WithMessage("MemberId is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
