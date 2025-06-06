using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant;

public sealed record GetSpecificApplicantQuery(Guid JobId, string MemberId)
    : IRequest<ErrorOr<ApiResponse<SpecificApplicantForSpecificJobResponse>>>;

public sealed record SpecificApplicantForSpecificJobResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int? GenderId,
    string Location,
    List<MemberEducationInJobResponse> Educations,
    List<MemberExperienceInJobResponse> Experiences);


public sealed record MemberEducationInJobResponse(
    Guid Id,
    string Level,
    string Faculty,
    string UniversityName);

public sealed record MemberExperienceInJobResponse(
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);