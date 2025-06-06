using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetApplicants;

public sealed class GetApplicantsQueryHandler(
    ICurrentUserProvider currentUserProvider,
    IBaseRepository<Member> memberRepository)
    : IRequestHandler<GetApplicantsQuery, ErrorOr<ApiResponse<IReadOnlyList<GetApplicantsResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<IReadOnlyList<GetApplicantsResponse>>>> Handle(GetApplicantsQuery request, CancellationToken cancellationToken)
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
                description: "Access denied. Only Company can add education.");
        }

        var response = await memberRepository.Entites()
            .AsQueryable()
            .AsNoTracking()
            .Include(m => m.JobApplications)
            .ThenInclude(ja => ja.Job)
            .Where(m => m.JobApplications.Any(ja =>
                ja.JobId == request.JobId && ja.Job.CompanyId == currentUser.UserId))
            .Select(m => new GetApplicantsResponse(
                m.Id,
                m.FullName,
                m.UserName!,
                m.PhoneNumber!,
                m.Email!
            )).ToListAsync(cancellationToken);

        return new ApiResponse<IReadOnlyList<GetApplicantsResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Employed Applicants retrieved successfully.",
            Data = response
        };
    }
}