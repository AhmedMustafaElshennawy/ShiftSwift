using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.shared;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;
using System.Globalization;

namespace ShiftSwift.Application.Features.rating.Commands.AddRating
{
    public sealed class AddRatingCommandHandler : IRequestHandler<AddRatingCommand, ErrorOr<ApiResponse<RatingResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Rating> _ratingRepository;

        public AddRatingCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Rating> ratingRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _ratingRepository = ratingRepository;
        }

        public async Task<ErrorOr<ApiResponse<RatingResponse>>> Handle(AddRatingCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
            }
            var currentUser = currentUserResult.Value;

            if (currentUser.UserId != request.RatedById)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: "Access denied. You can only submit a rating for yourself.");
            }

            var scoreString = request.Score.ToString(CultureInfo.InvariantCulture);
            if (scoreString.Contains('.') && scoreString.Split('.')[1].Length > 1)
            {
                return Error.Validation(
                    code: "Rating.InvalidFormat",
                    description: "Rating must have only one decimal.");
            }

            var existingRating = await _ratingRepository.Entites()
                .Where(r => r.CompanyId == request.CompanyId && r.RatedById == currentUser.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingRating is not null)
            {
                return Error.Conflict(
                    code: "Rating.AlreadyExists",
                    description: "You have already rated this company... Multiple ratings are not allowed.");
            }

            decimal roundedScore = Math.Round(request.Score, 1);

            var newRating = new Rating
            {
                Id = Guid.NewGuid(),
                CompanyId = request.CompanyId,
                RatedById = currentUser.UserId,
                Score = roundedScore,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Ratings.AddEntityAsync(newRating);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new RatingResponse(
                newRating.Id,
                newRating.CompanyId, 
                newRating.RatedById,
                newRating.Score,
                newRating.Comment,
            newRating.CreatedAt);

            return new ApiResponse<RatingResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Rating added successfully.",
                Data = response
            };
        }
    }
}

