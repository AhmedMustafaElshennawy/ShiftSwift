using FluentValidation;


namespace ShiftSwift.Application.Features.education.Commands.AddEducation;

public class AddEducationCommandValidator : AbstractValidator<AddEducationCommand>
{
    private static readonly string[] ValidLevels =
    [
        "High School", "Associate", "Bachelor", "Master", "Doctorate", "Professional"
    ];

    public AddEducationCommandValidator()
    {
        const int maxLength = 100;

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required.");

        RuleFor(x => x.Faculty)
            .NotEmpty().WithMessage("Institution is required.")
            .MaximumLength(maxLength).WithMessage($"Institution name must not exceed {maxLength} characters.");

        RuleFor(x => x.Level)
            .NotEmpty().WithMessage("Level of education is required.")
            .MaximumLength(maxLength).WithMessage($"Level of education cannot exceed {maxLength} characters.")
            .Must(level => ValidLevels.Contains(level))
            .WithMessage($"Invalid education level. Valid values: {string.Join(", ", ValidLevels)}");

        RuleFor(x => x.UniversityName)
            .NotEmpty().WithMessage("Degree is required.")
            .MaximumLength(maxLength).WithMessage($"Degree name must not exceed {maxLength} characters.");
    }
}