using ShiftSwift.Application.Features.education.Commands.AddEducation;
using ShiftSwift.Application.Features.education.Commands.DeleteEducation;
using ShiftSwift.Application.Features.education.Queries.GetEducation;
using ShiftSwift.Application.Features.experience.Commands.AddExperience;
using ShiftSwift.Application.Features.experience.Commands.DeleteEducation;
using ShiftSwift.Application.Features.experience.Queries.GetExperience;
using ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication;
using ShiftSwift.Application.Features.jobApplication.Query.ListMyJobApplicaions;
using ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;
using ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData;
using ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail;
using ShiftSwift.Application.Features.savedJobs.Commands.SaveJob;
using ShiftSwift.Application.DTOs.member;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace ShiftSwift.API.Controllers
{
    public class MemberController : ApiController
    {
        private readonly ISender _sender;
        public MemberController(ISender sender) => _sender = sender;

        [HttpPost("AddOrUpdateMamberProfileData/{MemberId}")]
        public async Task<IActionResult> AddOMamberProfileData([FromRoute] string MemberId, [FromBody] ProfileDTO request, CancellationToken cancellationToken)
        {
            var command = new AddMemberProfileDataCommand(
                MemberId,
                request.FirstName,
                request.MiddileName,
                request.LastName);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPost("AddOrUpdateEducation/{MemberId}")]
        public async Task<IActionResult> RegisterMember([FromRoute]string MemberId, [FromBody] EducationDTO request, CancellationToken cancellationToken)
        {
            var command = new AddEducationCommand(
                MemberId,
                request.Institution,
                request.Degree);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetEducation/{MemberId}")]
        public async Task<IActionResult> GetEducation([FromRoute] string MemberId, CancellationToken cancellationToken)
        {
            var query = new GetEducationQuery(MemberId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpDelete("DeleteEducation/{MemberId}")]
        public async Task<IActionResult> DeleteEducation([FromRoute] string MemberId, CancellationToken cancellationToken)
        {
            var command = new DeleteEducationCommand(MemberId);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPost("AddOrUpdateExperience/{MemberId}")]
        public async Task<IActionResult> AddExperience([FromRoute] string MemberId, [FromBody] ExperienceDTO request, CancellationToken cancellationToken)
        {
            var command = new AddExperienceCommand(MemberId,
                request.Title,
                request.CompanyName,
                request.StartDate,
                request.EndDate,
                request.Description);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetExperience/{MemberId}")]
        public async Task<IActionResult> GetExperience([FromRoute] string MemberId, CancellationToken cancellationToken)
        {
            var command = new GetExperienceQuery(MemberId);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpDelete("DeleteExperience/{MemberId}")]
        public async Task<IActionResult> DeleteExperience([FromRoute] string MemberId, CancellationToken cancellationToken)
        {
            var command = new DeleteExperienceCommand(MemberId);

            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPost("AddJobApplication")]
        public async Task<IActionResult> AddJobApplication(JobApplicationDTO request, CancellationToken cancellationToken)
        {
            var command = new CreateJobApplicationCommand(request.JobId,request.MemberId);
            var result = await _sender.Send(command, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetAllMyJobApplications/{MemberId}")]
        public async Task<IActionResult> GetJobApplications(string MemberId, CancellationToken cancellationToken)
        {
            var query = new ListMyJobApplicaionsQuery(MemberId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpPost("SaveJob")]
        public async Task<IActionResult> GetJobApplications(Guid JobId, string MemberId, CancellationToken cancellationToken)
        {
            var query = new SaveJobCommand(JobId,MemberId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }

        [HttpGet("GetAllSavedJobs/{MemberId}")]
        public async Task<IActionResult> GetAllSaveedJobs(string MemberId, CancellationToken cancellationToken)
        {
            var query = new GetSavedJobsQuery(MemberId);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }
        [HttpPost("ChangeMemberEmail/{MemberId}")]
        public async Task<IActionResult> ChangeEmail(string MemberId, string Email, CancellationToken cancellationToken)
        {
            var query = new ChangeMemberEmailCommand(MemberId, Email);

            var result = await _sender.Send(query, cancellationToken);
            var response = result.Match(
                success => Ok(result.Value),
                error => Problem(error));

            return response;
        }
    }
}