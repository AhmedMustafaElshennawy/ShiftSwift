using ShiftSwift.Application.Features.job.Commands.ApplyApplicant;
using ShiftSwift.Application.Features.job.Commands.DeletePostJob;
using ShiftSwift.Application.Features.job.Commands.PostJob;
using ShiftSwift.Application.Features.job.Commands.UpdatePostJob;
using ShiftSwift.Application.Features.job.Queries.GetAllJobPosts;
using ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;
using ShiftSwift.Application.Features.jobApplication.Query.GetMyLastWorkApplicants;
using ShiftSwift.Application.Features.ProfileData.Commands.ChangeCompanyEmail;
using ShiftSwift.Application.Features.jobApplication.Query.GetApplicants;
using ShiftSwift.Application.Features.rating.Queries.GetRating;
using ShiftSwift.Application.DTOs.Company;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ShiftSwift.Application.Features.job.Commands.RemoveFromShortlist;
using ShiftSwift.Application.Features.job.Queries.GetShortlistedMembers;
using ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant;


namespace ShiftSwift.API.Controllers;

public class CompanyController(ISender sender) : ApiController
{
    [HttpPost("AddOrUpdateCompanyProfileData")]
    public async Task<IActionResult> AddOrCompanyProfileData([FromBody] AddCompanyProfileDataCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

        return response;
    }

    [HttpPost("CreateJobPost/{CompanyId}")]
    public async Task<IActionResult> CreateJobPost([FromRoute] string CompanyId, [FromBody] JobDTO request,
        CancellationToken cancellationToken)
    {
        var command = new PostJobCommand(request.Title,
            request.Description,
            request.Location,
            request.JobType,
            request.WorkMode,
            request.Salary,
            request.SalaryType,
            request.Requirements,
            request.Keywords,
            request.Questions);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPut("UpdateJobPost/{JobId}")]
    public async Task<IActionResult> UpdateJobPost(Guid JobId, [FromBody] UpdateJobDTO request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePostJobCommand(JobId,
            request.Title,
            request.Description,
            request.Location,
            request.JobType,
            request.WorkMode,
            request.Salary,
            request.SalaryType,
            request.Requirements,
            request.Keywords,
            request.Questions);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpDelete("DeleteJobPost/{JobId}")]
    public async Task<IActionResult> DeleteJobPost(Guid JobId, CancellationToken cancellationToken)
    {
        var command = new DeletePostJobCommand(JobId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetAllApplicantsForSpecificJob/{JobId}")]
    public async Task<IActionResult> GetAllApplicantsForSpecificJob(Guid JobId, CancellationToken cancellationToken)
    {
        var query = new GetApplicantsQuery(JobId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetSpecificApplicantForSpecificJob/{JobId}/{MemberId}")]
    public async Task<IActionResult> GetSpecificApplicantForSpecificJob(Guid JobId, string MemberId,
        CancellationToken cancellationToken)
    {
        var query = new GetSpecificApplicantQuery(JobId, MemberId);

        var result = await sender.Send(query, cancellationToken);

        var response = result.Match(
            success => Ok(success),
            error => Problem(error));

        return response;
    }

    [HttpPost("ApplyApplicant/{JobId}")]
    public async Task<IActionResult> ApplyApplicant([FromRoute] Guid JobId, [FromQuery] string MemberId,
        [FromQuery] int status, CancellationToken cancellationToken)
    {
        var command = new ApplyApplicantCommand(JobId, MemberId, status);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetAllJobPostsForCompany/{CompanyId}")]
    public async Task<IActionResult> ApplyApplicant([FromRoute] string CompanyId,
        CancellationToken cancellationToken)
    {
        var command = new GetAllJobPostsForSpecificCompanyQuery(CompanyId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetShortlistedMembers/{jobId}")]
    public async Task<IActionResult> GetShortlistedMembers([FromRoute] Guid jobId,
        CancellationToken cancellationToken)
    {
        var query = new GetShortlistedMembersQuery(jobId);

        var result = await sender.Send(query, cancellationToken);

        var response = result.Match(
            success => Ok(success),
            error => Problem(error));

        return response;
    }

    [HttpPost("RemoveMemberFromShortlist/{JobId}")]
    public async Task<IActionResult> RemoveFromShortlist([FromRoute] Guid JobId, [FromQuery] string MemberId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFromShortlistCommand(JobId, MemberId);

        var result = await sender.Send(command, cancellationToken);

        var response = result.Match(
            success => Ok(success),
            error => Problem(error)
        );

        return response;
    }

    [HttpPost("ChangeCompanyEmail/{CompanyId}")]
    public async Task<IActionResult> ChangeEmail(string CompanyId, string Email,
        CancellationToken cancellationToken)
    {
        var query = new ChangeCompanyEmailCommand(CompanyId, Email);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetRating/{CompanyId}")]
    public async Task<IActionResult> GetAverageRating([FromRoute] string CompanyId,
        CancellationToken cancellationToken)
    {
        var query = new GetRatingQuery(CompanyId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetMyLastWorkApplicants/{CompanyId}")]
    public async Task<IActionResult> GetMyLastWorkApplicants([FromRoute] string CompanyId,
        CancellationToken cancellationToken)
    {
        var query = new GetMyLastWorkApplicantsQuery(CompanyId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }
}