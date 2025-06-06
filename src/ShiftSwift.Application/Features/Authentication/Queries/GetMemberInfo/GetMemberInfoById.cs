using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetMemberInfo;

public sealed record GetMemberInfoById(string Id) : IRequest<ErrorOr<ApiResponse<MemberResponseInfo>>>;


public sealed record GetMemberInfoByIdResponse(
    string MemberId,
    string FullName,
    string UserName,
    string PhoneNumber,
    string Email,
    int GenderId,
    string Location,
    List<GetMemberEducationResponse> Educations,
    List<GetMemberExperienceResponse> Experiences,
    List<GetMemberSkillResponse> Skills);


public sealed record GetMemberEducationResponse(
    Guid Id,
    string Level,
    string Faculty,
    string UniversityName);

public sealed record GetMemberExperienceResponse(
    string Title,
    string CompanyName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description);


public sealed record GetMemberSkillResponse(string Name);