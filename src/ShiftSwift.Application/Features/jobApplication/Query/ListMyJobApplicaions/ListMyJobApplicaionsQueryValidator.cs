using FluentValidation;

namespace ShiftSwift.Application.Features.jobApplication.Query.ListMyJobApplicaions
{
    public sealed class ListMyJobApplicaionsQueryValidator:AbstractValidator<ListMyJobApplicaionsQuery>
    {
        public ListMyJobApplicaionsQueryValidator()
        {
            RuleFor(x => x.MemberId)
              .NotEmpty().WithMessage("MemberId is required.")
              .Must(id => Guid.TryParse(id, out _)).WithMessage("MemberId must be a valid GUID.");
        }
    }
}
