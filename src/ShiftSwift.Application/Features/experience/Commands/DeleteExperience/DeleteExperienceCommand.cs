using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteExperience;

public sealed record DeleteExperienceCommand(string MemberId, Guid ExperienceId) :IRequest<ErrorOr<ApiResponse<Deleted>>>;