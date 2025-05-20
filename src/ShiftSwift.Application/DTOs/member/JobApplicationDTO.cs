using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.DTOs.member
{
    public sealed record JobApplicationDTO(Guid JobId,
        string MemberId,
        List<JobApplicationAnswerDTO> Answers);

    public sealed record JobApplicationAnswerDTO(
    Guid JobQuestionId,
    string? AnswerText,
    bool? AnswerBool);


    public sealed record JobApplicationResponse(Guid Id,
        Guid JobId, 
        string MemberId,
        DateTime AppliedOn,
        List<JobApplicationAnswerDTO> Answers);

}
