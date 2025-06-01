using FluentValidation;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication;

public class CreateJobApplicationCommandValidator : AbstractValidator<CreateJobApplicationCommand>
{
    public CreateJobApplicationCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty().WithMessage("JobId is required.")
            .NotEqual(Guid.Empty).WithMessage("JobId must be a valid GUID.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("MemberId must be a valid GUID.");

        RuleFor(x => x.Answers)
            .NotNull().WithMessage("Answers are required.")
            .Must(a => a.Any()).WithMessage("At least one answer is required.");

        RuleForEach(x => x.Answers).ChildRules(answer =>
        {
            answer.RuleFor(a => a.JobQuestionId)
                .NotEmpty().WithMessage("JobQuestionId is required.");

            answer.RuleFor(a => a)
                .Must(a => !string.IsNullOrWhiteSpace(a.AnswerText) || a.AnswerBool.HasValue)
                .WithMessage("Either AnswerText or AnswerBool must be provided.");
        });

    }
}