using FluentValidation;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;

public sealed class AddCompanyProfileDataCommandValidator:AbstractValidator<AddCompanyProfileDataCommand>
{
    public AddCompanyProfileDataCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("CompanyId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(100).WithMessage("FirstName cannot exceed 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(100).WithMessage("LastName cannot exceed 100 characters.");

        RuleFor(x => x.Overview)
            .MaximumLength(500).WithMessage("Overview cannot exceed 500 characters.");

        RuleFor(x => x.Field)
            .MaximumLength(155).WithMessage("Field cannot exceed 155 characters.");

        RuleFor(x => x.DateOfEstablish)
            .NotNull().WithMessage("DateOfEstablish is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("PhoneNumber is required.")
            .MaximumLength(20).WithMessage("PhoneNumber cannot exceed 20 characters.")
            .Matches(@"^\+?[0-9\s\-]{7,}$").WithMessage("Invalid PhoneNumber format.");

        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

        RuleFor(x => x.Area)
            .MaximumLength(100).WithMessage("Area cannot exceed 100 characters.");
    }
}