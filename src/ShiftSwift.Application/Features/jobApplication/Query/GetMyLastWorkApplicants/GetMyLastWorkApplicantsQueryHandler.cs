using ErrorOr;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.ApiResponse;
using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.identity;
using System.Net;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetMyLastWorkApplicants;

internal sealed class GetMyLastWorkApplicantsQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Member> memberRepository)
    : IRequestHandler<GetMyLastWorkApplicantsQuery,
        ErrorOr<ApiResponse<IReadOnlyList<GetMyLastWorkApplicantsResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<GetMyLastWorkApplicantsResponse>>>> Handle(
        GetMyLastWorkApplicantsQuery request, CancellationToken cancellationToken)
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
                description: "Access denied. Only Company can Retrive Applicants worked for them.");
        }

        //var lastAcceptedApplications = await memberRepository.Entites()
        //    .AsNoTracking()
        //    .SelectMany(m => m.JobApplications
        //        .Where(ja => ja.Status == (int)ApplicationStatus.Accepted &&
        //                     ja.Job.CompanyId == currentUser.UserId)
        //        .OrderByDescending(ja => ja.AppliedOn)
        //        .Take(10))
        //    .Include(ja => ja.Member)
        //    .Include(ja => ja.Job)
        //    .ToListAsync(cancellationToken);

        var response = await memberRepository.Entites()
            .AsQueryable()
            .AsNoTracking()
            .Where(m => m.JobApplications.Any(ja =>
                ja.Status == (int)ApplicationStatus.Accepted &&
                ja.Job.CompanyId == currentUser.UserId))
            .OrderByDescending(m => m.JobApplications
                .Where(ja => ja.Status == (int)ApplicationStatus.Accepted &&
                             ja.Job.CompanyId == currentUser.UserId)
                .Max(ja => ja.AppliedOn))
            .ProjectToType<GetMyLastWorkApplicantsResponse>()
            .ToListAsync(cancellationToken);

        return new ApiResponse<IReadOnlyList<GetMyLastWorkApplicantsResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Applicants retrieved successfully.",
            Data = response
        };
    }
}