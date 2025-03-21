using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.accomplishment.Queries.GetAccomplishment
{
    public sealed class GetAccomplishmentQueryHandler : IRequestHandler<GetAccomplishmentQuery, ErrorOr<ApiResponse<AccomplishmentResponse>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Accomplishment> _accomplishmentRepository;
        public GetAccomplishmentQueryHandler(ICurrentUserProvider currentUserProvider, IBaseRepository<Accomplishment> accomplishmentRepository)
        {
            _currentUserProvider = currentUserProvider;
            _accomplishmentRepository = accomplishmentRepository;
        }
        public async Task<ErrorOr<ApiResponse<AccomplishmentResponse>>> Handle(GetAccomplishmentQuery request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
            }

            var currentUser = currentUserResult.Value;
            if (!currentUser.Roles.Contains("Member"))
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Access denied. Only members can retrieve accomplishments.");
            }

            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            var currentUserAccomplishment = await _accomplishmentRepository.Entites()
                    .Where(x => x.MemberId == currentUser.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

            if (currentUserAccomplishment is null)
            {
                return new ApiResponse<AccomplishmentResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Accomplishment returned successfully.",
                    Data = "The current user has no accomplishments."
                };
            }

            var accomplishmentResponse = new AccomplishmentResponse(
                currentUserAccomplishment.Id,
                currentUser.UserId,
                currentUserAccomplishment.Title,
                currentUserAccomplishment.Description,
                currentUserAccomplishment.DateAchieved);

            return new ApiResponse<AccomplishmentResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Accomplishment returned successfully.",
                Data = accomplishmentResponse
            };
        }
    }
}
