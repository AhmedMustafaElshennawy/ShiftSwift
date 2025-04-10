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
using ShiftSwift.Application.Features.ProfileData.Commands.ChangeCompanyEmail;
using ShiftSwift.Application.Features.rating.Commands.AddRating;
using ShiftSwift.Application.Features.rating.Queries.GetRating;
using ShiftSwift.Application.Features.jobApplication.Query.GetMyLastWorkApplicants;
using ShiftSwift.Domain.Enums;



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

        [HttpPost("CreateJobPost/{CompanyId}")]
        public async Task<IActionResult> CreateJobPost([FromRoute] string CompanyId,[FromBody] JobDTO request, CancellationToken cancellationToken)
        {
            var command = new PostJobCommand(request.Title,
                request.Description, 
                request.Location,
                request.JobType);

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
        public async Task<IActionResult> ApplyApplicant([FromRoute]ApplicationStatus status ,[FromRoute] Guid JobId, [FromQuery] string MemberId, CancellationToken cancellationToken)
        {
            var command = new ApplyApplicantCommand(JobId, MemberId,status);

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

        [HttpPost("ChangeCompanyEmail/{CompanyId}")]
        public async Task<IActionResult> ChangeEmail(string CompanyId, string Email, CancellationToken cancellationToken)
        {
            var query = new ChangeCompanyEmailCommand(CompanyId, Email);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }  

        [HttpPost("AddRating/{CompanyId}")]
        public async Task<IActionResult> AddRating([FromRoute] string CompanyId, [FromQuery] string RatedById, [FromBody] RatingDTO request, CancellationToken cancellationToken)
        {
            var command = new AddRatingCommand(CompanyId, RatedById, request.Score, request.Comment);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match(
                success => Ok(success),
                error => Problem(error)
            );
        }

        [HttpGet("GetRating/{CompanyId}")]
        public async Task<IActionResult> GetAverageRating([FromRoute] string CompanyId, CancellationToken cancellationToken)
        {
            var query = new GetRatingQuery(CompanyId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetMyLastWorkApplicants/{CompanyId}")]
        public async Task<IActionResult> GetMyLastWorkApplicants([FromRoute] string CompanyId, CancellationToken cancellationToken)
        {
            var query = new GetMyLastWorkApplicantsQuery(CompanyId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }
    }
}