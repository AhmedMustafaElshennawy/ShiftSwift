using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.education.Commands.UpdateEducation;

public sealed record UpdateEducationCommand(
    string MemberId,
    string Level,
    string Faculty,
    string UniversityName) : IRequest<ErrorOr<ApiResponse<UpdateEducationRespone>>>;

public sealed record UpdateEducationRespone(
    Guid Id,
    string Level,
    string Faculty,
    string UniversityName);