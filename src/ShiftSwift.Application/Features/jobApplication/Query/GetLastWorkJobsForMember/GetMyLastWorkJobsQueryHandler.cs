using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.identity;
using System.Net;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetLastWorkJobsForMember;

internal sealed class GetMyLastWorkJobsQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Member> memberRepository)
    : IRequestHandler<GetMyLastWorkJobsQuery,
        ErrorOr<ApiResponse<IReadOnlyList<GetLastWorkJobResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<GetLastWorkJobResponse>>>> Handle(
        GetMyLastWorkJobsQuery request, CancellationToken cancellationToken)
    {
        var currentUserResult = await currentUserProvider.GetCurrentUser();
        if (currentUserResult.IsError)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
        }

        var currentUser = currentUserResult.Value;

        var jobs = await memberRepository.Entites()
            .AsNoTracking()
            .Include(m => m.JobApplications)
            .ThenInclude(ja => ja.Job)
            .ThenInclude(j => j.Company)
            .Where(m => m.Id == currentUser.UserId)
            .SelectMany(m => m.JobApplications)
            .Where(ja => ja.Status == 2)
            .ProjectToType<GetLastWorkJobResponse>()
            .ToListAsync(cancellationToken);

        return new ApiResponse<IReadOnlyList<GetLastWorkJobResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Past jobs retrieved successfully.",
            Data = jobs
        };
    }
}