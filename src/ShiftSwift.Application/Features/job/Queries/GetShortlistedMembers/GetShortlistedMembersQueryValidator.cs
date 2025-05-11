using FluentValidation;

namespace ShiftSwift.Application.Features.job.Queries.GetShortlistedMembers
{
    public sealed class GetShortlistedMembersQueryValidator : AbstractValidator<GetShortlistedMembersQuery>
    {
        public GetShortlistedMembersQueryValidator()
        {
            RuleFor(x => x.JobId)
                .NotEmpty().WithMessage("JobId is required.");
        }
    }
}

