using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.shared;
using ShiftSwift.Domain.Shared;
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
                    description: "Access denied. Only members can apply for jobs."); 
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

            var validQuestionIds = await _unitOfWork.JobQuestions.Entites()
                .Where(q => q.JobId == request.JobId)
                .Select(q => q.Id)
                .ToListAsync(cancellationToken);

            var invalidQuestionIds = request.Answers
                .Select(a => a.JobQuestionId)
                .Where(id => !validQuestionIds.Contains(id))
                .ToList();

            if (invalidQuestionIds.Any())
            {
                return Error.Validation(
                    code: "JobApplication.InvalidQuestions",
                    description: "One or more submitted question IDs are invalid.");
            }


            var jobApplication = new JobApplication
            {
                Id = Guid.NewGuid(),
                AppliedOn = DateTime.UtcNow,
                JobId = request.JobId,
                MemberId = memberId,
                Status = 1
            };

            await _unitOfWork.JobApplications.AddEntityAsync(jobApplication);


            foreach (var answerDto in request.Answers)
            {
                var answer = new ApplicationAnswer
                {
                    JobApplicationId = jobApplication.Id,
                    JobQuestionId = answerDto.JobQuestionId,
                    AnswerText = answerDto.AnswerText,
                    AnswerBool = answerDto.AnswerBool
                };
                await _unitOfWork.ApplicationAnswers.AddEntityAsync(answer);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new JobApplicationResponse(
                jobApplication.Id,
                jobApplication.JobId,
                jobApplication.MemberId,
                jobApplication.AppliedOn,
                request.Answers.Select(a => new JobApplicationAnswerDTO(
                    a.JobQuestionId,
                    a.AnswerText,
                    a.AnswerBool
                )).ToList()
            );

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
