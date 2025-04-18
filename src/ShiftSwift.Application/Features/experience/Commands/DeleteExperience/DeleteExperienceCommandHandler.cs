﻿using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteEducation
{
    public sealed class DeleteExperienceCommandHandler : IRequestHandler<DeleteExperienceCommand, ErrorOr<ApiResponse<Deleted>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Experience> _experienceRepository;
        public DeleteExperienceCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Experience> experienceRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _experienceRepository = experienceRepository;
        }
        public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
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

            var currentUserEducation = await _experienceRepository.Entites()
                    .Where(x => x.MemberId == currentUser.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

            if (currentUserEducation is null)
            {
                return Error.NotFound(
                    code: "Experience.NotFound",
                    description: "No Experience Found to Current User.");
            }
            var deletionResult = await _unitOfWork.Experiences.DeleteAsync(currentUserEducation);
            if (deletionResult is not true)
            {
                return Error.Failure(
                   code: "Experience.Failure",
                   description: "Failed To Delete Experience For Current User.");
            }
            await _unitOfWork.CompleteAsync(cancellationToken);
            return new ApiResponse<Deleted>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Message = "Experience Deleted successfully.",
                Data = null
            };
        }
    }
}
