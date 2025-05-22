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
        string Keywords,
        List<JobQuestionDTO> Questions);

    public sealed record UpdateJobDTO(string Title,
        string Description,
        string Location,
        int JobType,
        int WorkMode,
        decimal Salary,
        int SalaryType,
        string Requirements,
        string Keywords,
        List<UpdateJobQuestionDTO> Questions);

    public sealed record JobQuestionDTO( string QuestionText,
        int QuestionType);

    public sealed record UpdateJobQuestionDTO( Guid Id,
        string QuestionText,
        int QuestionType);


    public sealed record PostedJobResponse(string CompanyId,
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
        List<JobQuestionResponse> Questions);

    public sealed record JobQuestionResponse(Guid Id,
       string QuestionText,
       int QuestionType);

    public sealed record ListMyJobApplicaionsResponse(Guid JobId,
        string Title,
        string Description,
        string Location,
        DateTime PostedOn);
}
