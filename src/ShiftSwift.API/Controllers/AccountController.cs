using ShiftSwift.Application.Features.Authentication.Commands.LogOut;
using ShiftSwift.Application.Features.Authentication.Commands.Registermamber;
using ShiftSwift.Application.Features.Authentication.Queries.LogInCompany;
using ShiftSwift.Application.Features.Authentication.Queries.LogInMember;
using ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserInformation;
using ShiftSwift.Application.Features.Authentication.Commands.AddProfilePicture;
using ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserImageURL;
using ShiftSwift.Application.DTOs.identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;
using ShiftSwift.Application.Features.Authentication.Queries.GetCompanyInfo;
using ShiftSwift.Application.Features.Authentication.Queries.GetMemberInfo;

namespace ShiftSwift.API.Controllers;

public class AccountController(ISender sender) : ApiController
{
    [HttpPost("RegisterMember")]
    public async Task<IActionResult> RegisterMember([FromForm] RegisterMemberRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterMemberCommand(
            request.Email,
            request.UserName,
            request.Password,
            request.PhoneNumber);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("RegisterCompany")]
    public async Task<IActionResult> RegisterCompany([FromForm] RegisterCompanyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCompanyCommand(
            request.Email,
            request.UserName,
            request.Password,
            request.PhoneNumber);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("LoginCompany")]
    public async Task<IActionResult> LoginCompany([FromBody] LoginAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCompanyQuery(
            request.UserName,
            request.Password);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("LoginMember")]
    public async Task<IActionResult> LoginMember([FromBody] LoginAccountRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginMemberQuery(
            request.UserName,
            request.Password);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("LogOutAccoount")]
    public async Task<IActionResult> LogOutAccount(CancellationToken cancellationToken)
    {
        var command = new LogoutCommand();

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetCurrentUserInformation")]
    public async Task<IActionResult> GetCurrentUserInformation(CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserInformationQuery();

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpPost("AddOrUpdateProfilePicture")]
    public async Task<IActionResult> AddProfilePicture([FromForm] AddProfilePictureRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddProfilePictureCommand(request.FormFile);

        var result = await sender.Send(command, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetProfilePicture/{Id}")]
    public async Task<IActionResult> GetProfilePicture(string Id, CancellationToken cancellationToken)
    {
        var query = new GetCurrentUserImageURLQuery(Id);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetCompanyInfoById/{Id}")]
    public async Task<IActionResult> GetCompanyInfoById(string Id, CancellationToken cancellationToken)
    {
        var query = new GetCompanyInfoById(Id);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }

    [HttpGet("GetMemberInfoById/{Id}")]
    public async Task<IActionResult> GetMemberInfoById(string Id, CancellationToken cancellationToken)
    {
        var query = new GetMemberInfoById(Id);

        var result = await sender.Send(query, cancellationToken);
        var response = result.Match(
            success => Ok(result.Value),
            error => Problem(error));

        return response;
    }
}