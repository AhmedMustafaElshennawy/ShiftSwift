using FluentValidation;

namespace ShiftSwift.Application.Features.accomplishment.Queries.GetAccomplishment
{
    public sealed class GetAccomplishmentQueryValidator : AbstractValidator<GetAccomplishmentQuery>
    {
        public GetAccomplishmentQueryValidator()
        {
            RuleFor(X => X.MemberId).NotEmpty().WithMessage("MemberId is required.");
        }
    }
}
