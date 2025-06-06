using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Commands.PostJob;

public sealed record PostJobCommand(string Title,
    string Description,
    string Location,
    int JobType,
    int WorkMode,
    decimal Salary,
    int SalaryType,
    string Requirements,
    string Keywords) : IRequest<ErrorOr<ApiResponse<CompanyPostedJobResponse>>>;


public sealed record CompanyPostedJobResponse(string CompanyId,
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