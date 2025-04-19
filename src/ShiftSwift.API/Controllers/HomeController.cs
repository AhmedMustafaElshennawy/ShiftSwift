using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftSwift.Application.Features.job.Queries.GetRandomJobs;

namespace ShiftSwift.API.Controllers
{
    public class HomeController: ApiController
    {
        private readonly ISender _sender;
        public HomeController(ISender sender) => _sender = sender;

        [HttpGet("GetRandomJobs")]
        public async Task<IActionResult> GetRandomJobs([FromQuery] int PageNumber = 1,
            [FromQuery] int PageSize = 10,
            [FromQuery] string? SortBy = "JobType",
            [FromQuery] string? SortOrder = "asc",
            [FromQuery]int JobTypeIdFilterValue= 0,
            [FromQuery]int SalaryTypeIdFilterValue = 0,CancellationToken cancellationToken=default)
        {
            var query = new GetRandomJobsQuery
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                SortBy = SortBy,
                SortOrder = SortOrder!,
                JobTypeIdFilterValue = JobTypeIdFilterValue,
                SalaryTypeIdFilterValue = SalaryTypeIdFilterValue
            };

            var result = await _sender.Send(query,cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }
    }
}
