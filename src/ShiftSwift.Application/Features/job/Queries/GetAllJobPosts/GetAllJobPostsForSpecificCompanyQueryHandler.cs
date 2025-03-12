using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;


namespace ShiftSwift.Application.Features.job.Queries.GetAllJobPosts
{
    public sealed class GetAllJobPostsForSpecificCompanyQueryHandler : IRequestHandler<GetAllJobPostsForSpecificCompanyQuery, ErrorOr<ApiResponse<IReadOnlyList<PostedJobResponse>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        public GetAllJobPostsForSpecificCompanyQueryHandler(IUnitOfWork unitOfWork, ICurrentUserProvider currentUserProvider)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<ErrorOr<ApiResponse<IReadOnlyList<PostedJobResponse>>>> Handle(GetAllJobPostsForSpecificCompanyQuery request, CancellationToken cancellationToken)
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

            var jobs = await _unitOfWork.Jobs.Entites()
                  .Where(j => j.CompanyId == request.CompanyId)
                  .ToListAsync(cancellationToken);

            if (!jobs.Any())
            {
                return Error.NotFound(
                    code: "Jobs.NotFound",
                    description: "No job posts found for the specified company.");
            }

            var jobResponse = jobs.Select(job => new PostedJobResponse(
                job.CompanyId,
                job.Id,
                job.Title,
                job.Description,
                job.Location ,
                job.PostedOn
            )).ToList();

            return new ApiResponse<IReadOnlyList<PostedJobResponse>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Your Jobs is Retrevied successfully.",
                Data = jobResponse
            };
        }
    }
}
