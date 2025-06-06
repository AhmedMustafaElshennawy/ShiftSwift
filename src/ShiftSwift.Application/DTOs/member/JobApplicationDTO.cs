using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.DTOs.member;

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

public sealed record JobApplicationAnswerResponse(Guid JobQuestionId,
    string QuestionText,
    int QuestionType,
    string? AnswerText,
    bool? AnswerBool);

public sealed record SpecificApplicantResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int GenderId,
    string Location,
    List<MemberEducationResponse> Educations,
    List<MemberExperienceResponse> Experiences,
    List<MemberSkillResponse> Skills,
    List<JobApplicationAnswerResponse> Answers);