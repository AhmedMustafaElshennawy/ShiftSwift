using FluentValidation;

namespace ShiftSwift.Application.Features.education.Commands.UpdateEducation;

public sealed class UpdateEducationCommandValidator : AbstractValidator<UpdateEducationCommand>
{
    public UpdateEducationCommandValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required.")
            .MaximumLength(450).WithMessage("Member ID cannot exceed 450 characters.");

        RuleFor(x => x.LevelOfEducation)
            .NotEmpty().WithMessage("Level of education is required.")
            .MaximumLength(100).WithMessage("Level of education cannot exceed 100 characters.")
            .Must(BeValidEducationLevel).WithMessage("Invalid level of education specified.");

        RuleFor(x => x.FieldOfStudy)
            .NotEmpty().WithMessage("Field of study is required.")
            .MaximumLength(150).WithMessage("Field of study cannot exceed 150 characters.");


        RuleFor(x => x.SchoolName)
            .NotEmpty().WithMessage("School name is required.")
            .MaximumLength(200).WithMessage("School name cannot exceed 200 characters.");
    }

    private bool BeValidEducationLevel(string level)
    {
        var validLevels = new[] { "High School", "Associate", "Bachelor", "Master", "Doctorate", "Professional" };
        return validLevels.Contains(level);
    }
}