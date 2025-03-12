using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.experience.Commands.AddExperience
{
    public sealed class AddExperienceCommandHandler : IRequestHandler<AddExperienceCommand, ErrorOr<ApiResponse<AddExperienceResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Experience> _experienceRepository;
        public AddExperienceCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Experience> experienceRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _experienceRepository = experienceRepository;
        }
        public async Task<ErrorOr<ApiResponse<AddExperienceResponse>>> Handle(AddExperienceCommand request, CancellationToken cancellationToken)
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
            var CurrentUserExperience = await _experienceRepository.Entites()
                .Where(X => X.MemberId == currentUser.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            bool isUpdated = CurrentUserExperience is not null;
            if (isUpdated)
            {
                CurrentUserExperience!.Title = request.Title;
                CurrentUserExperience.CompanyName = request.CompanyName;
                CurrentUserExperience.StartDate = request.StartDate;
                CurrentUserExperience.EndDate = request.EndDate;
                CurrentUserExperience.Description = request.Description;
                    
                await _unitOfWork.Experiences.UpdateAsync(CurrentUserExperience); 
            }
            else
            {
                var newExperience = new Experience
                {
                    Id = Guid.NewGuid(),
                    MemberId = currentUser.UserId,
                    Title = request.Title,
                    CompanyName = request.CompanyName,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Description = request.Description,
                };
                await _unitOfWork.Experiences.AddEntityAsync(newExperience);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            var response = new AddExperienceResponse(
                currentUser.UserId,
                request.Title,
                request.CompanyName,
                request.StartDate,
                request.EndDate,
                request.Description);

            return new ApiResponse<AddExperienceResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = isUpdated ? "Experience updated successfully." : "Experience added successfully.",
                Data = response
            };
        }
    }
}
