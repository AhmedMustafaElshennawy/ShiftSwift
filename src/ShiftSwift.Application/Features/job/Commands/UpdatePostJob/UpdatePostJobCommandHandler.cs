using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.Shared;
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
                    description: "Access denied. Only companies can update job posts");
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

            var existingQuestions = await _unitOfWork.JobQuestions
                .Entites()
                .Where(q => q.JobId == job.Id)
                .ToListAsync(cancellationToken);

            var requestQuestionIds = request.Questions?
                .Where(q => q.Id != Guid.Empty)
                .Select(q => q.Id)
                .ToList() ?? new List<Guid>();

            foreach (var existingQuestion in existingQuestions)
            {
                if (!requestQuestionIds.Contains(existingQuestion.Id))
                {
                    await _unitOfWork.JobQuestions.DeleteAsync(existingQuestion);
                }
            }

            if (request.Questions != null && request.Questions.Any())
            {
                foreach (var question in request.Questions)
                {
                    if (question.Id != Guid.Empty)
                    {
                        var existingQuestion = existingQuestions.FirstOrDefault(q => q.Id == question.Id);
                        if (existingQuestion != null)
                        {
                            existingQuestion.QuestionText = question.QuestionText;
                            existingQuestion.QuestionType = (QuestionType)question.QuestionType;
                            await _unitOfWork.JobQuestions.UpdateAsync(existingQuestion);
                        }
                    }
                    else
                    {
                        var newQuestion = new JobQuestion
                        {
                            JobId = job.Id,
                            QuestionText = question.QuestionText,
                            QuestionType = (QuestionType)question.QuestionType
                        };
                        await _unitOfWork.JobQuestions.AddEntityAsync(newQuestion);
                    }
                }
            }
            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new PostedJobResponse(currentUser.UserId,
                job.Id,
                job.Title,
                job.Description,
                job.Location,
                job.PostedOn,
                job.JobTypeId,
                job.WorkModeId,
                job.Salary,
                job.SalaryTypeId,
                job.Requirements,
                job.Keywords,
                job.Questions.Select(q => new JobQuestionResponse(
                  q.Id,
                  q.QuestionText,
             (int)q.QuestionType)).ToList())
;

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