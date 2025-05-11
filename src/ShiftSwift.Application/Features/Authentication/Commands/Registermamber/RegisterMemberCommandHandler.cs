using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Application.services.Email;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Commands.Registermamber;

public sealed class RegisterMemberCommandHandler : IRequestHandler<RegisterMemberCommand,
    ErrorOr<ApiResponse<RegisterationMemberResult>>>
{
    private readonly string _defaultMemberRole = "Member";
    private readonly UserManager<Account> _userManager;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterMemberCommandHandler(
        UserManager<Account> userManager,
        IEmailService emailService,
        ITokenGenerator tokenGenerator,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
        _roleManager = roleManager;
    }

<<<<<<< HEAD
    public async Task<ErrorOr<ApiResponse<RegisterationMemberResult>>> Handle(RegisterMemberCommand request,
        CancellationToken cancellationToken)
    {
        var roleExists = await _roleManager.RoleExistsAsync(_defaultMemberRole);
        if (!roleExists)
=======
        public RegisterMemberCommandHandler(
            UserManager<Account> userManager,
            IEmailService emailService,
            ITokenGenerator tokenGenerator,
            RoleManager<IdentityRole> roleManager)
>>>>>>> ebaa3ef3ec038adccd13e663f0e79a280dcdc049
        {
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(_defaultMemberRole));
            if (!roleResult.Succeeded)
            {
                var errorMessages = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Error.Failure(
                    code: "IdentityRole.Failure",
                    description: $"Failed to create role: {errorMessages}");
            }
        }

        if (await _userManager.FindByEmailAsync(request.Email) is not null)
        {
            return Error.Forbidden(
                code: "Email.Forbidden",
                description: $"This Email is already registered.");
        }

        if (await _userManager.FindByNameAsync(request.UserName) is not null)
        {
            return Error.Forbidden(
                code: "UserName.Forbidden",
                description: $"This username is already registered.");
        }

        var member = new Member
        {
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            GenderId = 3
        };

        var result = await _userManager.CreateAsync(member, request.Password);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
            return Error.Unauthorized(
                code: "result.Failure",
                description: $"User creation failed: {errorMessages}");
        }

        var roleAssignmentResult = await _userManager.AddToRoleAsync(member, _defaultMemberRole);
        if (!roleAssignmentResult.Succeeded)
        {
            var errorMessages = string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description));
            return Error.Failure(
                code: "RoleAssignment.Failure",
                description: $"Failed to assign role: {errorMessages}");
        }

        var memberToken = await _tokenGenerator.GenerateToken(member, _defaultMemberRole);
        var MemberResponse = new MemberResponse(member.Id,
            member.FullName,
            member.UserName,
            member.PhoneNumber!,
            member.Email,
            member.GenderId.Value,
            member.Location);

        var registerationMemberResponse = new RegisterationMemberResult(
            MemberResponse: MemberResponse,
            token: memberToken);

        return new ApiResponse<RegisterationMemberResult>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "User registered successfully",
            Data = registerationMemberResponse
        };
    }
}