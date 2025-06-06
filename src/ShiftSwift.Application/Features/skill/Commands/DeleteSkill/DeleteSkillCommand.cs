﻿using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.skill.Commands.DeleteSkill;

public sealed record DeleteSkillCommand(string MemberId, Guid SkillId) : IRequest<ErrorOr<ApiResponse<Deleted>>>;