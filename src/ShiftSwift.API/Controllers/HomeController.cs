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
        public async Task<IActionResult> GetRandomJobs(CancellationToken cancellationToken)
        {
            var query = new GetRandomJobsQuery();

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));


            return response;
        }
    }
}
