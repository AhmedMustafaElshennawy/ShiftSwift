﻿using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.rating.Queries.GetRating 
{
    public sealed record GetRatingQuery(string CompanyId)
        : IRequest<ErrorOr<ApiResponse<AverageRatingResponse>>>;
}
