using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.skill.Queries.GetSkill
{
    public sealed record GetSkillQuery(string MemberId) : IRequest<ErrorOr<ApiResponse<List<SkillResponse>>>>;
}

