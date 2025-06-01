using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.Net;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.searchJobs.Queries.SearchJobs
{
    public sealed class SearchJobsQueryHandler : IRequestHandler<SearchJobsQuery, ErrorOr<ApiResponse<PaginatedResponse<SearchJobsResponse>>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Member> _memberRepository;

        public SearchJobsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Member> memberRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _memberRepository = memberRepository;
        }

        public async Task<ErrorOr<ApiResponse<PaginatedResponse<SearchJobsResponse>>>> Handle(
            SearchJobsQuery request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
            }

            var currentUser = currentUserResult.Value;

            if (!currentUser.Roles.Contains("Member"))
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Access denied. Only Members can search for jobs.");
            }

            var memberExists = await _memberRepository.Entites()
                .AnyAsync(m => m.Id == currentUser.UserId, cancellationToken);

            if (!memberExists)
            {
                return Error.NotFound(
                    code: "User.NotFound",
                    description: "Member not found.");
            }

            var query = _unitOfWork.Jobs.Entites()
                .Include(x => x.Company)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.ToLower();
                query = query.Where(j =>
                    j.Title.ToLower().Contains(search) ||
                    j.Keywords.ToLower().Contains(search));
            }

            if (request.JobTypeIdFilterValue > 0)
            {
                query = query.Where(x => x.JobTypeId == request.JobTypeIdFilterValue);
            }

            if (request.MinSalary.HasValue)
            {
                query = query.Where(x => x.Salary >= request.MinSalary.Value);
            }

            if (request.MaxSalary.HasValue)
            {
                query = query.Where(x => x.Salary <= request.MaxSalary.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Location))
            {
                var location = request.Location.ToLower();
                query = query.Where(j => j.Location.ToLower().Contains(location));
            }

            switch (request.SortBy?.ToLower())
            {
                case "latest":
                    query = query.OrderByDescending(x => x.PostedOn);
                    break;
                case "oldest":
                    query = query.OrderBy(x => x.PostedOn);
                    break;
                default:
                    query = query.OrderByDescending(x => x.PostedOn);
                    break;
            }

            var pagedJobs = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            if (!pagedJobs.Any())
            {
                return Error.NotFound(
                    code: "jobs.NotFound",
                    description: "No jobs found with the selected filters.");
            }

            var jobsResponse = pagedJobs.Select(job => new SearchJobsResponse(
                job.Id,
                job.CompanyId,
                job.Company?.UserName ?? "Unknown",
                job.Title,
                job.Description,
                job.Location,
                job.PostedOn,
                job.Salary,
                request.MinSalary,
                request.MaxSalary,
                job.JobTypeId

            )).ToList();

            var totalCount = await query.CountAsync(cancellationToken);

            return new ApiResponse<PaginatedResponse<SearchJobsResponse>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Jobs retrieved successfully.",
                Data = PaginatedResponse<SearchJobsResponse>.Create(
                    jobsResponse,
                    totalCount,
                    request.PageNumber,
                    request.PageSize)
            };
        }
    }
}
