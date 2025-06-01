using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.ApiResponse;

namespace ShiftSwift.Application.Features.searchJobs.Queries.SearchJobs
{
   public sealed record SearchJobsQuery : PaginatedRequest, IRequest<ErrorOr<ApiResponse<PaginatedResponse<SearchJobsResponse>>>>
   {
     public string? Search { get; init; }
     public string? Location { get; init; }
     public double? MaxDistanceKm { get; init; }
     public decimal? MinSalary { get; init; }
     public decimal? MaxSalary { get; init; }
     public int JobTypeIdFilterValue { get; init; }
   }
}