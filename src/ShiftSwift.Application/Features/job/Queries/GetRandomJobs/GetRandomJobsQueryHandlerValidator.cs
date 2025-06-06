using FluentValidation;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Application.Features.job.Queries.GetRandomJobs;

public sealed class GetRandomJobsQueryHandlerValidator:AbstractValidator<GetRandomJobsQuery>
{
    public GetRandomJobsQueryHandlerValidator()
    {
        RuleFor(x => x.JobTypeIdFilterValue)
            .Must(BeAValidJobType)
            .WithMessage("Invalid JobTypeIdFilterValue. Allowed values are 0(nothing), 1 (FullTime), 2 (PartTime),  3 (Freelance).");

        RuleFor(x => x.SalaryTypeIdFilterValue)
            .InclusiveBetween(0, 3)
            .WithMessage("SalaryTypeIdFilterValue must be between 1 and 3")
            .WithMessage("Salary Type . Allowed values:0(nothing), 1 (PerMonth), 2 (PerHour), 3 (Contract).");

        RuleFor(x => x.SortOrder)
            .Must(order => order.ToLower() == "asc" || order.ToLower() == "desc")
            .WithMessage("SortOrder must be 'asc' or 'desc'.");
    }
    private bool BeAValidJobType(int jobTypeId)
    {
        return jobTypeId switch
        {
            0 or 1 or 2 or 3 => true,
            _ => false
        };
    }
}