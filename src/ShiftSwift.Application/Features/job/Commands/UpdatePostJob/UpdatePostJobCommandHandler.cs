using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.job.Commands.UpdatePostJob
{
    public sealed class UpdatePostJobCommandHandler : IRequestHandler<UpdatePostJobCommand, ErrorOr<ApiResponse<PostedJobResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public UpdatePostJobCommandHandler(IUnitOfWork unitOfWork,ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<PostedJobResponse>>> Handle(UpdatePostJobCommand request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
            }

            var currentUser = currentUserResult.Value;
            if (!currentUser.Roles.Contains("Company"))
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Access denied. Only members can add education.");
            }

            var job = await _unitOfWork.Jobs.Entites()
                .Where(j => j.Id == request.JobId)
                .FirstOrDefaultAsync(cancellationToken);

            if (job is null)
            {
                return new ApiResponse<PostedJobResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = $"No Jobs Found To Your Company With Id You Entered: {request.JobId}",
                    Data = null
                };
            }

            if (job.CompanyId != currentUser.UserId)
            {
                return Error.Forbidden(
                    code: "Job.Forbidden",
                    description: "You are not allowed to Update this job.");
            }

            job.Title = request.Title;
            job.Description = request.Description;
            job.Location = request.Location;

            await _unitOfWork.Jobs.UpdateAsync(job);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new PostedJobResponse(currentUser.UserId,
                job.Id,
                job.Title,
                job.Description,
                job.Location,
                job.PostedOn,
                job.JobType);

            return new ApiResponse<PostedJobResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Your Job is Updated successfully.",
                Data = response
            };
        }
    }
}