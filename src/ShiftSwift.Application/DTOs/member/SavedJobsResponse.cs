namespace ShiftSwift.Application.DTOs.member
{
    public sealed record SavedJobsResponse(
        Guid Id,
        Guid JobId,
        string JobTitle,
        string CompanyName,
        DateTime SavedOn,
        string CompanyId,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn,
        int SalaryTypeId,
        decimal Salary,
        int JobTypeTd);
}