using ShiftSwift.Domain.Enums;

namespace ShiftSwift.Application.DTOs.member;

public sealed record MemberResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int GenderId,
    string Location);

public sealed record GetApplicantsResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email);

public sealed record ApplyApplicantResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int status);


public sealed record MemberResponseInfo(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int GenderId,
    string Location,
    DateTime DateOfBirth,
    List<MemberEducationResponse> Educations,
    List<MemberExperienceResponse> Experiences,
    List<MemberSkillResponse> Skills);

public sealed record MemberEducationResponse(
    Guid Id,
    string Level,
    string Faculty,
    string UniversityName);

public sealed record MemberExperienceResponse(
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);


public sealed record MemberSkillResponse(string Name);