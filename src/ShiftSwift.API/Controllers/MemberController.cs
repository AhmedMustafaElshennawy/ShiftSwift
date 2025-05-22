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
using ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment;
using ShiftSwift.Application.Features.accomplishment.Commands.DeleteAccomplishment;
using ShiftSwift.Application.Features.accomplishment.Queries.GetAccomplishment;
using ShiftSwift.Application.Features.skill.Commands.AddSkill;
using ShiftSwift.Application.Features.skill.Commands.DeleteSkill;
using ShiftSwift.Application.Features.skill.Queries.GetSkill;
using ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail;
using ShiftSwift.Application.Features.savedJobs.Commands.SaveJob;
using ShiftSwift.Application.Features.searchJobs.Queries.SearchJobs;
using ShiftSwift.Application.DTOs.member;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ShiftSwift.Application.Features.education.Commands.UpdateEducation;
using ShiftSwift.Application.Features.skill.Commands.UpdateSkill;
using ShiftSwift.Application.Features.experience.Commands.UpdateExperience;

namespace ShiftSwift.API.Controllers;

public class MemberController(ISender sender) : ApiController
{
    private readonly ISender _sender = sender;

    [HttpPost("AddOrUpdateMamberProfileData/{MemberId}")]
    public async Task<IActionResult> AddMamberProfileData([FromRoute] string MemberId,
        [FromBody] ProfileDTO request, CancellationToken cancellationToken)
    {
        var command = new AddMemberProfileDataCommand(
            MemberId,
            request.FirstName,
            request.Location,
            request.LastName,
            request.GenderId);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("AddEducation/{MemberId}")]
    public async Task<IActionResult> AddEducation([FromRoute] string MemberId, [FromBody] EducationDto request,
        CancellationToken cancellationToken)
    {
        var command = new AddEducationCommand(
            MemberId,
            request.Level,
            request.Faculty,
            request.UniversityName);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPut("UpdateEducation/{MemberId}")]
    public async Task<IActionResult> UpdateEducation([FromRoute] string MemberId, [FromBody] EducationDto request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEducationCommand(
            MemberId,
            request.Level,
            request.Faculty,
            request.UniversityName);

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
    public async Task<IActionResult> DeleteEducation([FromRoute] string MemberId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteEducationCommand(MemberId);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("AddExperience/{MemberId}")]
    public async Task<IActionResult> AddExperience([FromRoute] string MemberId, [FromBody] ExperienceDTO request,
        CancellationToken cancellationToken)
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

    [HttpPut("UpdateExperience/{MemberId}")]
    public async Task<IActionResult> UpdateExperience([FromRoute] string MemberId,
        [FromBody] UpdateExperienceDTO request,
        CancellationToken cancellationToken)
    {

        var command = new UpdateExperienceCommand(request.ExperienceId,
            MemberId,
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
    public async Task<IActionResult> DeleteExperience([FromRoute] string MemberId, [FromBody] Guid experienceId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteExperienceCommand(MemberId, experienceId);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("AddSkill/{MemberId}")]
    public async Task<IActionResult> AddSkill([FromRoute] string MemberId, [FromBody] SkillDTO request,
        CancellationToken cancellationToken)
    {
        var command = new AddSkillCommand(
            MemberId,
            request.Name);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPut("UpdateSkill/{MemberId}")]
    public async Task<IActionResult> UpdateSkill([FromRoute] string MemberId, [FromBody] UpdateSkillDTO request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSkillCommand(
            MemberId,
            request.SkillId,
            request.Name);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }


    [HttpGet("GetSkills/{MemberId}")]
    public async Task<IActionResult> GetSkills([FromRoute] string MemberId, CancellationToken cancellationToken)
    {
        var query = new GetSkillQuery(MemberId);

        var result = await _sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpDelete("DeleteSkill/{MemberId}")]
    public async Task<IActionResult> DeleteSkill([FromRoute] string MemberId, [FromBody] Guid skillId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSkillCommand(MemberId,skillId);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("AddOrUpdateAccomplishment/{MemberId}")]
    public async Task<IActionResult> AddOrUpdateAccomplishment([FromRoute] string MemberId,
        [FromBody] AccomplishmentDTO request, CancellationToken cancellationToken)
    {
        var command = new AddAccomplishmentCommand(
            MemberId,
            request.Title,
            request.Description,
            request.DateAchieved);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetAccomplishments/{MemberId}")]
    public async Task<IActionResult> GetAccomplishments([FromRoute] string MemberId,
        CancellationToken cancellationToken)
    {
        var query = new GetAccomplishmentQuery(MemberId);

        var result = await _sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpDelete("DeleteAccomplishment/{MemberId}")]
    public async Task<IActionResult> DeleteAccomplishment([FromRoute] string MemberId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAccomplishmentCommand(MemberId);

        var result = await _sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }


    [HttpPost("AddJobApplication")]
    public async Task<IActionResult> AddJobApplication(JobApplicationDTO request,
        CancellationToken cancellationToken)
    {
        var command = new CreateJobApplicationCommand(request.JobId, request.MemberId);
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
    public async Task<IActionResult> GetJobApplications(Guid JobId, string MemberId,
        CancellationToken cancellationToken)
    {
        var query = new SaveJobCommand(JobId, MemberId);

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