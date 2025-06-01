using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.identity;
using ShiftSwift.Domain.shared;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.rating.Queries.GetRating
{
    public sealed class GetRatingQueryHandler
        : IRequestHandler<GetRatingQuery, ErrorOr<ApiResponse<AverageRatingResponse>>>
    {
        private readonly IBaseRepository<Company> _companyRepository;
        private readonly IBaseRepository<Rating> _ratingRepository;

        public GetRatingQueryHandler(
            IBaseRepository<Company> companyRepository,
            IBaseRepository<Rating> ratingRepository)
        {
            _companyRepository = companyRepository;
            _ratingRepository = ratingRepository;
        }

        public async Task<ErrorOr<ApiResponse<AverageRatingResponse>>> Handle(
            GetRatingQuery request, CancellationToken cancellationToken)
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
                        AverageScore: null,
                        Ratings: new List<CompanyRatingResponse>())
                };
            }

             var ratings = await _ratingRepository.Entites()
                 .Where(r => r.CompanyId == request.CompanyId)
                 .OrderByDescending(r => r.CreatedAt)
                 .Include(r => r.RatedBy)  
                 .Select(r => new CompanyRatingResponse(
                        r.Score,                   
                        r.Comment,                 
                        r.CreatedAt,
                        r.RatedBy.UserName!,
                        r.RatedBy.ImageUrl))
                 .ToListAsync(cancellationToken);


            var averageRating = await _ratingRepository.Entites()
                .Where(x => x.CompanyId == request.CompanyId)
                .AverageAsync(x => (decimal?)x.Score, cancellationToken);

            var averageRatingResponse = new AverageRatingResponse(
                CompanyId: request.CompanyId,
                Ratings: ratings,
                AverageScore: averageRating.HasValue ? Math.Round(averageRating.Value, 1) : null
            );

            return new ApiResponse<AverageRatingResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "company rating retrieved successfully.",
                Data = averageRatingResponse
            };
        }
    }
}
