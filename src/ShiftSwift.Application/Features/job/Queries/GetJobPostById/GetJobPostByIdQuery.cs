using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Queries.GetJobPostById;

public sealed record GetJobPostByIdQuery(Guid JobId)
    : IRequest<ErrorOr<ApiResponse<PostedJobByIdResponse>>>;


public sealed record PostedJobByIdResponse(string CompanyId,
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