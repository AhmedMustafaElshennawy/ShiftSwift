using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.memberprofil;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.savedJobs.Commands.SaveJob
{
    public sealed class SaveJobCommandHandler:IRequestHandler<SaveJobCommand,ErrorOr<ApiResponse<bool>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public SaveJobCommandHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<bool>>> Handle(SaveJobCommand request, CancellationToken cancellationToken)
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

            if (request.MemberId != currentUser.UserId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. Id you entered :{request.MemberId} is wrong.");
            }

            var job = await _unitOfWork.Jobs.Entites()
                .Where(X => X.Id == request.JobId)
                .FirstOrDefaultAsync(cancellationToken);

            if (job is null)
            {
                return Error.NotFound(
                    code: "Job.NotFound",
                    description: "The specified job does not exist.");
            }

            var isAlreadySaved = await _unitOfWork.SavedJobs.Entites()
            .AnyAsync(s => s.MemberId == request.MemberId && s.JobId == request.JobId, cancellationToken);

            if (isAlreadySaved)
            {
                return Error.Conflict(
                    code: "Job.AlreadySaved",
                    description: "The job has already been saved by this member.");
            }

            var savedJob = new SavedJob
            {
                Id = Guid.NewGuid(),
                MemberId = request.MemberId,
                JobId = request.JobId,
                SavedOn = DateTime.UtcNow
            };

            await _unitOfWork.SavedJobs.AddEntityAsync(savedJob);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new ApiResponse<bool>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Job Is Saved successfully.",
                Data = true
            };
        }
    }
}
