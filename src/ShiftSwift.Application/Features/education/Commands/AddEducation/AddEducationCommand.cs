using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;


namespace ShiftSwift.Application.Features.education.Commands.AddEducation;

public sealed record AddEducationCommand(string MemberId,
    string Level,
    string Faculty,
    string UniversityName) :IRequest<ErrorOr<ApiResponse<EducationRespone>>>;