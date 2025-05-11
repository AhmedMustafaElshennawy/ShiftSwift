using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.skill.Commands.UpdateSkill;

public sealed record UpdateSkillCommand(
    string MemberId,
    Guid SkillId,
    string Name) : IRequest<ErrorOr<ApiResponse<SkillResponse>>>;

public sealed record SkillResponse(
    string MemberId,
    string Name);