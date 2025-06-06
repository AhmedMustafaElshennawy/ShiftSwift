using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication;

public sealed record CreateJobApplicationCommand(
    Guid JobId, 
    string MemberId) : IRequest<ErrorOr<ApiResponse<AddJobApplicationResponse>>>;

public sealed record AddJobApplicationResponse(Guid Id,
    Guid JobId,
    string MemberId,
    DateTime AppliedOn);