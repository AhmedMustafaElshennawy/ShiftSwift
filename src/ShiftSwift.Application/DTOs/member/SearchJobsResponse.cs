namespace ShiftSwift.Application.DTOs.member 
{
    public sealed record SearchJobsResponse(
        Guid Id,
        string CompanyId,
        string CompanyName,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn,
        decimal Salary,
        decimal? MinSalary,
        decimal? MaxSalary,
        int JobTypeId
    );
}
