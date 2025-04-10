using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication
{
    public sealed record CreateJobApplicationCommand(
        Guid JobId, 
        string MemberId,
        ApplicationStatus ApplicationStatus) :IRequest<ErrorOr<ApiResponse<JobApplicationResponse>>>;

}
