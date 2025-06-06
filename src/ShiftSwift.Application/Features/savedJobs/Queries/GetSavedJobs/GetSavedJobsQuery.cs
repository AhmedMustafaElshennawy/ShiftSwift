using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;

public sealed record GetSavedJobsQuery(string MemberId)
    :IRequest<ErrorOr<ApiResponse<IReadOnlyList<GetAllSavedJobsResponse>>>>;


public sealed record GetAllSavedJobsResponse(
    Guid Id,
    Guid JobId,
    string JobTitle,
    DateTime SavedOn,
    string CompanyId,
    string Title,
    string Description,
    string Location,
    DateTime PostedOn,
    int SalaryTypeId,
    decimal Salary,
    int JobTypeTd);