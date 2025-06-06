using FluentValidation;

namespace ShiftSwift.Application.Features.job.Commands.RemoveFromShortlist;

public class RemoveFromShortlistCommandValidator : AbstractValidator<RemoveFromShortlistCommand>
{
    public RemoveFromShortlistCommandValidator()
    {
        RuleFor(x => x.JobId)
            .NotEmpty().WithMessage("Job ID is required.");

        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("Member ID is required.");
    }
}