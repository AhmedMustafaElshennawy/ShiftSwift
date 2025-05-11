using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.education.Commands.UpdateEducation;

public sealed record UpdateEducationCommand(
    string MemberId,
    string LevelOfEducation,
    string FieldOfStudy,
    string SchoolName) : IRequest<ErrorOr<ApiResponse<UpdateEducationRespone>>>;

public sealed record UpdateEducationRespone(
    Guid Id,
    string SchoolName,
    string LevelOfEducation,
    string FieldOfStudy);