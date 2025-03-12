using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.experience.Queries.GetExperience
{
    public sealed class GetExperienceQueryHandler : IRequestHandler<GetExperienceQuery, ErrorOr<ApiResponse<ExperienceResponse>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Experience> _experienceRepository;
        public GetExperienceQueryHandler(ICurrentUserProvider currentUserProvider,IBaseRepository<Experience> experienceRepository)
        {
            _currentUserProvider = currentUserProvider;
            _experienceRepository = experienceRepository;
        }
        public async Task<ErrorOr<ApiResponse<ExperienceResponse>>> Handle(GetExperienceQuery request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only members can add education.");
            }

            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            var currentUserExperience = await _experienceRepository.Entites()
                    .Where(x => x.MemberId == currentUser.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

            if (currentUserExperience is null)
            {
                return new ApiResponse<ExperienceResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Experience Rturned successfully.",
                    Data = "The Current User Has No Experience."
                };
            }

            var experienceResponse = new ExperienceResponse(currentUser.UserId,
                currentUserExperience.Title,
                currentUserExperience.CompanyName,
                currentUserExperience.StartDate,
                currentUserExperience.EndDate,
                currentUserExperience.Description);

            return new ApiResponse<ExperienceResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Experience Rturned successfully.",
                Data = experienceResponse
            };
        }
    }
}
