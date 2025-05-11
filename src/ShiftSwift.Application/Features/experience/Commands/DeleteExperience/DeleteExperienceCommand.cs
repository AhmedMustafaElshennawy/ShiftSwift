using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteEducation;

public sealed record DeleteExperienceCommand(string MemberId, Guid ExperienceId) :IRequest<ErrorOr<ApiResponse<Deleted>>>;