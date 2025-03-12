using FluentValidation;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData
{
    public sealed class AddCompanyProfileDataCommandValidator:AbstractValidator<AddCompanyProfileDataCommand>
    {
        public AddCompanyProfileDataCommandValidator()
        {
            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("CompanyId is required.");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("CompanyName is required.")
                .MaximumLength(100).WithMessage("CompanyName cannot exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
