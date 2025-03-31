using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.shared;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.rating.Queries.GetRating
{
    public sealed class GetAverageRatingQueryHandler
        : IRequestHandler<GetAverageRatingQuery, ErrorOr<ApiResponse<AverageRatingResponse>>>
    {
        private readonly IBaseRepository<Company> _companyRepository;
        private readonly IBaseRepository<Rating> _ratingRepository;

        public GetAverageRatingQueryHandler(
            IBaseRepository<Company> companyRepository,
            IBaseRepository<Rating> ratingRepository)
        {
            _companyRepository = companyRepository;
            _ratingRepository = ratingRepository;
        }

        public async Task<ErrorOr<ApiResponse<AverageRatingResponse>>> Handle(
            GetAverageRatingQuery request, CancellationToken cancellationToken)
        {
            var companyExists = await _companyRepository.Entites()
                .AnyAsync(c => c.Id == request.CompanyId, cancellationToken);

            if (!companyExists)
            {
                return Error.NotFound(
                    code: "Company.NotFound",
                    description: "The requested company does not exist.");
            }

            bool hasRatings = await _ratingRepository.Entites()
                .AnyAsync(x => x.CompanyId == request.CompanyId, cancellationToken);

            if (!hasRatings)
            {
                return new ApiResponse<AverageRatingResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "No ratings found for this company.",
                    Data = new AverageRatingResponse(
                        CompanyId: request.CompanyId,
                        AverageScore: null)
                };
            }

            var averageRating = await _ratingRepository.Entites()
                .Where(x => x.CompanyId == request.CompanyId)
                .AverageAsync(x => (decimal?)x.Score, cancellationToken);

            var averageRatingResponse = new AverageRatingResponse(
                CompanyId: request.CompanyId,
                AverageScore: averageRating.HasValue ? Math.Round(averageRating.Value, 1) : null
            );

            return new ApiResponse<AverageRatingResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Average company rating retrieved successfully.",
                Data = averageRatingResponse
            };
        }
    }
}
