using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetAllJobPosts;

public sealed record GetAllJobPostsForSpecificCompanyQuery(string CompanyId)
    : IRequest<ErrorOr<ApiResponse<IReadOnlyList<GetAllPostedJobForSpecificCompanyResponse>>>>;


public sealed record GetAllPostedJobForSpecificCompanyResponse(string CompanyId,
    Guid JobId,
    string Title,
    string Description,
    string Location,
    DateTime PostedOn,
    int JobType,
    int WorkMode,
    decimal Salary,
    int SalaryType,
    string Requirements,
    string Keywords);