using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Extentions;
using System.Net;
using ErrorOr;
using MediatR;
using ShiftSwift.Domain.ApiResponse;


namespace ShiftSwift.Application.Features.job.Queries.GetRandomJobs;

public sealed class GetRandomJobsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRandomJobsQuery, ErrorOr<ApiResponse<PaginatedResponse<GetRandomJobsResponse>>>>
{
    public async Task<ErrorOr<ApiResponse<PaginatedResponse<GetRandomJobsResponse>>>> Handle(GetRandomJobsQuery request,
        CancellationToken cancellationToken)
    {
        var query = unitOfWork.Jobs.Entites()
            .Include(x => x.Company)
            .AsQueryable();

        if (request.JobTypeIdFilterValue > 0)
        {
            query = query.Where(x => x.JobTypeId == request.JobTypeIdFilterValue);
        }

        if (request.SalaryTypeIdFilterValue > 0)
        {
            query = query.Where(x => x.SalaryTypeId == request.SalaryTypeIdFilterValue);
        }

        switch (request.SortBy?.ToLower())
        {
            case "jobtype":
                query = request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.JobTypeId)
                    : query.OrderBy(x => x.JobTypeId);
                break;

            case "salarytype":
                query = request.SortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.SalaryTypeId)
                    : query.OrderBy(x => x.SalaryTypeId);
                break;

            default:
                query = query.OrderBy(x => Guid.NewGuid());
                break;
        }

        var paginatedJobs = await query.ToPaginatedListAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken);

        if (paginatedJobs is null || !paginatedJobs.Data.Any())
        {
            var filterMessage = new List<string>();
            if (request.JobTypeIdFilterValue > 0)
                filterMessage.Add($"job type: {request.JobTypeIdFilterValue}");

            if (request.SalaryTypeIdFilterValue > 0)
                filterMessage.Add($"salary type: {request.SalaryTypeIdFilterValue}");

            return Error.NotFound(
                code: "jobs.NotFound",
                description: filterMessage.Any()
                    ? $"No jobs found with {string.Join(" and ", filterMessage)}."
                    : "No jobs found.");
        }

        var jobs = paginatedJobs.Data.Select(x => new GetRandomJobsResponse(
            x.Id,
            x.CompanyId,
            x.Title,
            x.Description,
            x.Location,
            x.PostedOn,
            x.SalaryTypeId,
            x.Salary,
            x.JobTypeId
        )).ToList();

        var response = PaginatedResponse<GetRandomJobsResponse>.Create(
            jobs,
            paginatedJobs.TotalCount,
            paginatedJobs.CurrentPage,
            paginatedJobs.PageSize);

        return new ApiResponse<PaginatedResponse<GetRandomJobsResponse>>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Jobs retrieved successfully.",
            Data = response
        };
    }
}