using FluentValidation;


namespace ShiftSwift.Application.Features.education.Commands.AddEducation
{
    public class AddEducationCommandValidator : AbstractValidator<AddEducationCommand>
    {
        public AddEducationCommandValidator()
        {
            RuleFor(X=>X.MemberId).NotEmpty().WithMessage("MemberId Is required.");
            RuleFor(x => x.Institution)
                .NotEmpty().WithMessage("Institution is required.")
                .MaximumLength(100).WithMessage("Institution name must not exceed 100 characters.");

            RuleFor(x => x.Degree)
                .NotEmpty().WithMessage("Degree is required.")
                .MaximumLength(100).WithMessage("Degree name must not exceed 100 characters.");
        }
    }
}
