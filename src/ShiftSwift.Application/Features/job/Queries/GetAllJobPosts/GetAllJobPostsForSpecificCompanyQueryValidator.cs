using FluentValidation;

namespace ShiftSwift.Application.Features.job.Queries.GetAllJobPosts;

public sealed class GetAllJobPostsForSpecificCompanyQueryValidator:AbstractValidator<GetAllJobPostsForSpecificCompanyQuery>
{
    public GetAllJobPostsForSpecificCompanyQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("CompanyId is required.")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("CompanyId must be a valid GUID.");
    }
}