using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.shared;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication
{
    public sealed class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand, ErrorOr<ApiResponse<JobApplicationResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<JobApplication> _jobApplicationRepository;
        public CreateJobApplicationCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<JobApplication> jobApplicationRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _jobApplicationRepository = jobApplicationRepository;
        }
        public async Task<ErrorOr<ApiResponse<JobApplicationResponse>>> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only members can add Job Application.");
            }

            var job = await _unitOfWork.Jobs.Entites()
                .Where(J=>J.Id ==request.JobId)
                .FirstOrDefaultAsync(cancellationToken);

            if (job is null)
            {
                return Error.NotFound(
                    code: "Job.NotFound",
                    description: $"No Job Found With Id You Entred {request.JobId}.");
            }

            var memberId = request.MemberId ?? currentUser.UserId;
            var existingApplication = await _jobApplicationRepository.Entites()
                .AsQueryable()
                .AnyAsync(ja => ja.JobId == request.JobId && ja.MemberId == memberId, cancellationToken);

            if (existingApplication)
            {
                return Error.Conflict(
                    code: "JobApplication.Duplicate",
                    description: "You have already applied for this job.");
            }

            var jobApplicaion = new JobApplication
            {
                Id = Guid.NewGuid(),
                AppliedOn = DateTime.UtcNow,
                JobId = request.JobId,
                MemberId = request.MemberId ?? currentUser.UserId,
                Status = false,
            };
            await _jobApplicationRepository.AddEntityAsync(jobApplicaion);
            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new JobApplicationResponse(jobApplicaion.Id,
                jobApplicaion.JobId,
                jobApplicaion.MemberId,
                jobApplicaion.AppliedOn);

            return new ApiResponse<JobApplicationResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Message = "JobApplication added successfully.",
                Data = response
            };
        }
    }
}