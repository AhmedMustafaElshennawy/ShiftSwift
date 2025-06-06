using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.job.Queries.GetAllJobPosts;

public sealed class GetAllJobPostsForSpecificCompanyQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserProvider currentUserProvider)
    : IRequestHandler<GetAllJobPostsForSpecificCompanyQuery,
        ErrorOr<ApiResponse<IReadOnlyList<GetAllPostedJobForSpecificCompanyResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<GetAllPostedJobForSpecificCompanyResponse>>>> Handle(
        GetAllJobPostsForSpecificCompanyQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = await currentUserProvider.GetCurrentUser();
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

        var jobs = await unitOfWork.Jobs.Entites()
            .Where(j => j.CompanyId == request.CompanyId)
            .ToListAsync(cancellationToken);

        if (!jobs.Any())
        {
            return Error.NotFound(
                code: "Jobs.NotFound",
                description: "No job posts found for the specified company.");
        }

        var jobResponse = jobs.Adapt<List<GetAllPostedJobForSpecificCompanyResponse>>();

        return new ApiResponse<IReadOnlyList<GetAllPostedJobForSpecificCompanyResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Jobs retrieved successfully.",
            Data = jobResponse
        };
    }
}