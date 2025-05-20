
namespace ShiftSwift.Application.DTOs.Company;
public sealed record JobInfoResponse(
    string CompanyId,
    Guid JobId,
    string Title,
    string Description,
    string Location,
    DateTime PostedOn,
    int JobType,
    int WorkMode,
    decimal Salary,
    int SalaryType,
    string Requirements,
    string Keywords,
    List<JobQuestionDTO> Questions
);