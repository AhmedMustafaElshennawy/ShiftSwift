using ShiftSwift.Application.Features.job.Commands.ApplyApplicant;
using ShiftSwift.Application.Features.job.Commands.DeletePostJob;
using ShiftSwift.Application.Features.job.Commands.PostJob;
using ShiftSwift.Application.Features.job.Commands.UpdatePostJob;
using ShiftSwift.Application.Features.job.Queries.GetAllJobPosts;
using ShiftSwift.Application.Features.jobApplication.Query.GetApplicants;
using ShiftSwift.Application.DTOs.Company;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;


namespace ShiftSwift.API.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ISender _sender;
        public CompanyController(ISender sender) => _sender = sender;

        [HttpPost("AddOrUpdateCompanyProfileData/{CompanyId}")]
        public async Task<IActionResult> AddOrCompanyProfileData([FromRoute] string CompanyId, [FromBody] CompanyProfileDataDTO request, CancellationToken cancellationToken)
        {
            var command = new AddCompanyProfileDataCommand(
                CompanyId,
                request.CompanyName,
                request.Description);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPost("CreateJobPost")]
        public async Task<IActionResult> CreateJobPost([FromBody] JobDTO request, CancellationToken cancellationToken)
        {
            var command = new PostJobCommand(request.Title,
                request.Description, 
                request.Location);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPut("UpdateJobPost/{JobId}")]
        public async Task<IActionResult> UpdateJobPost(Guid JobId, [FromBody] JobDTO request, CancellationToken cancellationToken)
        {
            var command = new UpdatePostJobCommand(JobId,
                request.Title,
                request.Description,
                request.Location);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpDelete("DeleteJobPost/{JobId}")]
        public async Task<IActionResult> DeleteJobPost(Guid JobId, CancellationToken cancellationToken)
        {
            var command = new DeletePostJobCommand(JobId);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetAllApplicantsForSpecificJob/{JobId}")]
        public async Task<IActionResult> GetAllApplicantsForSpecificJob(Guid JobId, CancellationToken cancellationToken)
        {
            var query = new GetApplicantsQuery(JobId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPost("ApplyApplicant/{JobId}")]
        public async Task<IActionResult> ApplyApplicant([FromRoute] Guid JobId, [FromQuery] string MemberId, CancellationToken cancellationToken)
        {
            var command = new ApplyApplicantCommand(JobId, MemberId);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetAllJobPostsForCompany/{CompanyId}")]
        public async Task<IActionResult> ApplyApplicant([FromRoute] string CompanyId, CancellationToken cancellationToken)
        {
            var command = new GetAllJobPostsForSpecificCompanyQuery(CompanyId);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }
    }
}