using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;


namespace ShiftSwift.Application.Features.education.Commands.AddEducation
{
    public sealed record AddEducationCommand(string MemberId,
        string LevelOfEducation,
        string FieldOfStudy,
        string SchoolName) :IRequest<ErrorOr<ApiResponse<EducationRespone>>>;
}
