using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.DTOs.Company
{
    public sealed record JobDTO(string Title,
        string Description,
        string Location,
        int JobType,
        int WorkMode,
        decimal Salary, 
        int SalaryType, 
        string Requirements,
        string Keywords);

    public sealed record PostedJobResponse(string CompanyId,
        Guid JobId,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn,
        int JobType);

    public sealed record ListMyJobApplicaionsResponse(Guid JobId,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn);
}
