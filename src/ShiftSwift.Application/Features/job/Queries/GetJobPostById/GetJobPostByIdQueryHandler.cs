using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.job.Queries.GetJobPostById
{
    public sealed class GetJobPostByIdQueryHandler : IRequestHandler<GetJobPostByIdQuery, ErrorOr<ApiResponse<PostedJobResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetJobPostByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<ApiResponse<PostedJobResponse>>> Handle(GetJobPostByIdQuery request, CancellationToken cancellationToken)
        {
            var job = await _unitOfWork.Jobs.Entites()
                .Include(j => j.Questions)
                .FirstOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);

            if (job is null)
            {
                return Error.NotFound(
                    code: "Job.NotFound",
                    description: "Job post not found.");
            }

            var jobResponse = new PostedJobResponse(
                job.CompanyId,
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
                    (int)q.QuestionType
                )).ToList()
            );

            return new ApiResponse<PostedJobResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Job post retrieved successfully.",
                Data = jobResponse
            };
        }
    }
}
