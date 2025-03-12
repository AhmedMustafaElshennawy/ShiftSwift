using FluentValidation;

namespace ShiftSwift.Application.Features.experience.Commands.AddExperience
{
    public sealed class AddExperienceCommandValidator : AbstractValidator<AddExperienceCommand>
    {
        public AddExperienceCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(100).WithMessage("Company name must not exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date cannot be in the future.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("End date must be later than start date.")
                .When(x => x.EndDate.HasValue);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(X => X.MemberId).NotEmpty().WithMessage("MemberId Is required.");

        }
    }
}
