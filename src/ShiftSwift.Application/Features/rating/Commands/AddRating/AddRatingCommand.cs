using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;
using ShiftSwift.Application.DTOs.Company;

namespace ShiftSwift.Application.Features.rating.Commands.AddRating
{
    public sealed record AddRatingCommand(string CompanyId, string RatedById, decimal Score, string? Comment)
        : IRequest<ErrorOr<ApiResponse<RatingResponse>>>;
}

