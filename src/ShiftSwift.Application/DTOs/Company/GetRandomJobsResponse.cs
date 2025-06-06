namespace ShiftSwift.Application.DTOs.Company;

public sealed record GetRandomJobsResponse(
    Guid Id,
    string CompanyId,
    string Title,
    string Description,
    string Location,
    DateTime PostedOn,
    int SalaryTypeId,
    decimal Salary,
    int JobTypeTd
);