using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using System.Net;
using Mapster;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant;

public sealed class GetSpecificApplicantQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetSpecificApplicantQuery, ErrorOr<ApiResponse<SpecificApplicantForSpecificJobResponse>>>
{
    public async Task<ErrorOr<ApiResponse<SpecificApplicantForSpecificJobResponse>>> Handle(GetSpecificApplicantQuery request, CancellationToken cancellationToken)
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
                description: "companies can access applicant details.");
        }

        var applicant = await unitOfWork.JobApplications.Entites()
            .AsNoTracking()
            .Include(ja => ja.Job)
            .Include(ja => ja.Member).ThenInclude(m => m.Educations)
            .Include(ja => ja.Member).ThenInclude(m => m.Experiences)
            .Where(ja => ja.JobId == request.JobId && ja.MemberId == request.MemberId)
            .Where(ja => ja.Job.CompanyId == currentUser.UserId)
            .Select(ja => ja.Member) // only need the Member part
            .ProjectToType<SpecificApplicantForSpecificJobResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (applicant is null)
        {
            return Error.NotFound(
                code: "Application.NotFound",
                description: "No such application for this member and job under your company.");
        }

        return new ApiResponse<SpecificApplicantForSpecificJobResponse>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Applicant details retrieved successfully.",
            Data = applicant
        };
    }
}