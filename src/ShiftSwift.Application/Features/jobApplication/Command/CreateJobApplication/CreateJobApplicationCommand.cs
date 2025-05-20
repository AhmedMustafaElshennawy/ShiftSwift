using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication
{
    public sealed record CreateJobApplicationCommand(
        Guid JobId, 
        string MemberId,
        List<JobApplicationAnswerDTO> Answers) : IRequest<ErrorOr<ApiResponse<JobApplicationResponse>>>;
}
