using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetJobPostById;

internal sealed class GetJobPostByIdQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetJobPostByIdQuery, ErrorOr<ApiResponse<PostedJobByIdResponse>>>
{
    public async Task<ErrorOr<ApiResponse<PostedJobByIdResponse>>> Handle(GetJobPostByIdQuery request,
        CancellationToken cancellationToken)
    {
        var job = await unitOfWork.Jobs.Entites()
            .FirstOrDefaultAsync(j => j.Id == request.JobId, cancellationToken);

        if (job is null)
        {
            return Error.NotFound(
                code: "Job.NotFound",
                description: "Job post not found.");
        }

        var jobResponse = job.Adapt<PostedJobByIdResponse>();

        return new ApiResponse<PostedJobByIdResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Job post retrieved successfully.",
            Data = jobResponse
        };
    }
}