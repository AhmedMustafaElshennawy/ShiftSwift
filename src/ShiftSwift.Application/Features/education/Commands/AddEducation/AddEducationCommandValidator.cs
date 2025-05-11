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
        RuleFor(X=>X.MemberId).NotEmpty().WithMessage("MemberId Is required.");
        RuleFor(x => x.SchoolName)
            .NotEmpty().WithMessage("Institution is required.")
            .MaximumLength(100).WithMessage("Institution name must not exceed 100 characters.");


        RuleFor(x => x.LevelOfEducation)
            .NotEmpty().WithMessage("Level of education is required.")
            .MaximumLength(100).WithMessage("Level of education cannot exceed 100 characters.")
            .Must(BeValidEducationLevel)
            .WithMessage($"Invalid level of education specified. Valid values are: {string.Join(", ", ValidLevels)}");


        RuleFor(x => x.FieldOfStudy)
            .NotEmpty().WithMessage("Degree is required.")
            .MaximumLength(100).WithMessage("Degree name must not exceed 100 characters.");
    }
    private bool BeValidEducationLevel(string level)
    {
        return ValidLevels.Contains(level);
    }
}