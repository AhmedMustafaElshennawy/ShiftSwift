using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.Extentions;
using ShiftSwift.Shared.paging;

namespace ShiftSwift.Application.Features.job.Queries.GetRandomJobs
{
    public sealed class GetRandomJobsQueryHandler : IRequestHandler<GetRandomJobsQuery, ErrorOr<PaginatedResponse<GetRandomJobsResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetRandomJobsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<ErrorOr<PaginatedResponse<GetRandomJobsResponse>>> Handle(GetRandomJobsQuery request, CancellationToken cancellationToken)
        {
            var jobRepository = _unitOfWork.Jobs.Entites();

            var query = jobRepository
            .AsQueryable()
            .Include(x => x.Company)
            .OrderBy(x => Guid.NewGuid())
            .Select(x => new GetRandomJobsResponse(
                x.Id,
                x.CompanyId,
                x.Company != null ? x.Company.CompanyName : "Unknown",
                x.Title,
                x.Description,
                x.Location,
                x.PostedOn
            ));

            var paginatedJobs = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

            return paginatedJobs.Data.Count > 0
            ? paginatedJobs
            : Error.NotFound(
                code: "jobs.NotFound", 
                description:"No jobs found.");

        }
    }
}