using FluentValidation;

namespace ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;

public sealed class GetSavedJobsQueryValidator:AbstractValidator<GetSavedJobsQuery>
{
    public GetSavedJobsQueryValidator()
    {
        RuleFor(x => x.MemberId)
            .NotEmpty().WithMessage("MemberId is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("MemberId must be a valid GUID.");
    }
}