﻿using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs
{
    public sealed record GetSavedJobsQuery(string MemberId)
        :IRequest<ErrorOr<ApiResponse<IReadOnlyList<SavedJobsResponse>>>>;
}
