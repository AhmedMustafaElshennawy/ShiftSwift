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
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ShiftSwift.Application.Features.job.Commands.RemoveFromShortlist;
using ShiftSwift.Application.Features.job.Queries.GetShortlistedMembers;
using ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant;


namespace ShiftSwift.API.Controllers;

public sealed class CompanyController(ISender sender) : ApiController
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

    [HttpPost("CreateJobPost")]
    public async Task<IActionResult> CreateJobPost([FromBody] PostJobCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            Problem);

        return response;
    }

    [HttpPut("UpdateJobPost")]
    public async Task<IActionResult> UpdateJobPost([FromBody] UpdatePostJobCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            Problem);

        return response;
    }

    [HttpDelete("DeleteJobPost/{JobId}")]
    public async Task<IActionResult> DeleteJobPost(Guid JobId, CancellationToken cancellationToken)
    {
        var command = new DeletePostJobCommand(JobId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            Problem);

        return response;
    }

    [HttpGet("GetAllApplicantsForSpecificJob/{JobId}")]
    public async Task<IActionResult> GetAllApplicantsForSpecificJob(Guid JobId, CancellationToken cancellationToken)
    {
        var query = new GetApplicantsQuery(JobId);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            Problem);

        return response;
    }

    [HttpGet("GetSpecificApplicantForSpecificJob")]
    public async Task<IActionResult> GetSpecificApplicantForSpecificJob([FromQuery]GetSpecificApplicantQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value), Problem);

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
            Problem);

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
            Problem);

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
            Problem);

        return response;
    }

    [HttpPost("RemoveMemberFromShortlist/{JobId}")]
    public async Task<IActionResult> RemoveFromShortlist([FromRoute] Guid JobId, [FromQuery] string MemberId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFromShortlistCommand(JobId, MemberId);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(success => Ok(success), Problem);

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
            Problem);

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
            Problem);

        return response;
    }

    [HttpGet("GetMyLastWorkApplicants")]
    public async Task<IActionResult> GetMyLastWorkApplicants([FromQuery] GetMyLastWorkApplicantsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            Problem);

        return response;
    }
}