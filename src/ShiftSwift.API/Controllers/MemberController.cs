using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.Features.accomplishment.Commands.AddAccomplishment;
using ShiftSwift.Application.Features.accomplishment.Commands.DeleteAccomplishment;
using ShiftSwift.Application.Features.accomplishment.Queries.GetAccomplishment;
using ShiftSwift.Application.Features.education.Commands.AddEducation;
using ShiftSwift.Application.Features.education.Commands.DeleteEducation;
using ShiftSwift.Application.Features.education.Commands.UpdateEducation;
using ShiftSwift.Application.Features.education.Queries.GetEducation;
using ShiftSwift.Application.Features.experience.Commands.AddExperience;
using ShiftSwift.Application.Features.experience.Commands.DeleteExperience;
using ShiftSwift.Application.Features.experience.Commands.UpdateExperience;
using ShiftSwift.Application.Features.experience.Queries.GetExperience;
using ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication;
using ShiftSwift.Application.Features.jobApplication.Query.GetLastWorkJobsForMember;
using ShiftSwift.Application.Features.jobApplication.Query.ListMyJobApplicaions;
using ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData;
using ShiftSwift.Application.Features.ProfileData.Commands.ChangeMemberEmail;
using ShiftSwift.Application.Features.rating.Commands.AddRating;
using ShiftSwift.Application.Features.savedJobs.Commands.SaveJob;
using ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;
using ShiftSwift.Application.Features.searchJobs.Queries.SearchJobs;
using ShiftSwift.Application.Features.skill.Commands.AddSkill;
using ShiftSwift.Application.Features.skill.Commands.DeleteSkill;
using ShiftSwift.Application.Features.skill.Commands.UpdateSkill;
using ShiftSwift.Application.Features.skill.Queries.GetSkill;

namespace ShiftSwift.API.Controllers;

public sealed class MemberController(ISender sender) : ApiController
{
    [HttpPost("AddOrUpdateMamberProfileData")]
    public async Task<IActionResult> AddMamberProfileData([FromBody] AddMemberProfileDataCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(success => Ok(result.Value),Problem);
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

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

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

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpGet("GetEducation/{MemberId}")]
    public async Task<IActionResult> GetEducation([FromRoute] string MemberId, CancellationToken cancellationToken)
    {
        var query = new GetEducationQuery(MemberId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpDelete("DeleteEducation/{MemberId}")]
    public async Task<IActionResult> DeleteEducation([FromRoute] string MemberId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteEducationCommand(MemberId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

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

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

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

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpGet("GetExperience/{MemberId}")]
    public async Task<IActionResult> GetExperience([FromRoute] string MemberId, CancellationToken cancellationToken)
    {
        var command = new GetExperienceQuery(MemberId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpDelete("DeleteExperience/{MemberId}")]
    public async Task<IActionResult> DeleteExperience([FromRoute] string MemberId, [FromBody] Guid experienceId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteExperienceCommand(MemberId, experienceId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpPost("AddSkill/{MemberId}")]
    public async Task<IActionResult> AddSkill([FromRoute] string MemberId, [FromBody] SkillDTO request,
        CancellationToken cancellationToken)
    {
        var command = new AddSkillCommand(
            MemberId,
            request.Name);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

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

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }


    [HttpGet("GetSkills/{MemberId}")]
    public async Task<IActionResult> GetSkills([FromRoute] string MemberId, CancellationToken cancellationToken)
    {
        var query = new GetSkillQuery(MemberId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpDelete("DeleteSkill/{MemberId}")]
    public async Task<IActionResult> DeleteSkill([FromRoute] string MemberId, [FromBody] Guid skillId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSkillCommand(MemberId, skillId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

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

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpGet("GetAccomplishments/{MemberId}")]
    public async Task<IActionResult> GetAccomplishments([FromRoute] string MemberId,
        CancellationToken cancellationToken)
    {
        var query = new GetAccomplishmentQuery(MemberId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpDelete("DeleteAccomplishment/{MemberId}")]
    public async Task<IActionResult> DeleteAccomplishment([FromRoute] string MemberId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAccomplishmentCommand(MemberId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }


    [HttpPost("AddJobApplication")]
    public async Task<IActionResult> AddJobApplication(CreateJobApplicationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(success => Ok(result.Value), Problem);
        return response;
    }

    [HttpGet("GetAllMyJobApplications/{MemberId}")]
    public async Task<IActionResult> GetJobApplications(string MemberId, CancellationToken cancellationToken)
    {
        var query = new ListMyJobApplicaionsQuery(MemberId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpPost("SaveJob")]
    public async Task<IActionResult> GetJobApplications(Guid JobId, string MemberId,
        CancellationToken cancellationToken)
    {
        var query = new SaveJobCommand(JobId, MemberId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpGet("GetAllSavedJobs/{MemberId}")]
    public async Task<IActionResult> GetAllSaveedJobs(string MemberId, CancellationToken cancellationToken)
    {
        var query = new GetSavedJobsQuery(MemberId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpPost("ChangeMemberEmail/{MemberId}")]
    public async Task<IActionResult> ChangeEmail(string MemberId, string Email, CancellationToken cancellationToken)
    {
        var query = new ChangeMemberEmailCommand(MemberId, Email);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpGet("SearchJobs")]
    public async Task<IActionResult> SearchJobs(
        [FromQuery(Name = "search")] string? search,
        [FromQuery(Name = "area")] string? area,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = "latest",
        [FromQuery] int jobTypeIdFilterValue = 0,
        [FromQuery] decimal? minSalary = null,
        [FromQuery] decimal? maxSalary = null,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchJobsQuery
        {
            Search = search,
            Location = area,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SortBy = sortBy!,
            JobTypeIdFilterValue = jobTypeIdFilterValue,
            MinSalary = minSalary,
            MaxSalary = maxSalary
        };

        var result = await sender.Send(query, cancellationToken);

        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpPost("AddRating/{CompanyId}")]
    public async Task<IActionResult> AddRating([FromRoute] string CompanyId, [FromQuery] string RatedById,
        [FromBody] RatingDTO request, CancellationToken cancellationToken)
    {
        var command = new AddRatingCommand(CompanyId, RatedById, request.Score, request.Comment);
        var result = await sender.Send(command, cancellationToken);

        return result.Match(success => Ok(result.Value), Problem);
    }

    [HttpPost("GetLastWork")]
    public async Task<IActionResult> GetLastWork(GetMyLastWorkJobsQuery query, CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        return result.Match(success => Ok(result.Value), Problem);
    }
}