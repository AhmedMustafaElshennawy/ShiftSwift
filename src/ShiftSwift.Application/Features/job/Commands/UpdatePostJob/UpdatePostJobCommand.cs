using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.job.Commands.UpdatePostJob;

public sealed record UpdatePostJobCommand(Guid JobId,
    string Title,
    string Description,
    string Location,
    int JobType,
    int WorkMode,
    decimal Salary,
    int SalaryType,
    string Requirements,
    string Keywords) : IRequest<ErrorOr<ApiResponse<UpdatePostedJobResponse>>>;


public sealed record UpdatePostedJobResponse(string CompanyId,
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