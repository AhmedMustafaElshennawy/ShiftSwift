using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;
using System.Security.Claims;

namespace ShiftSwift.Application.Features.education.Commands.AddEducation;

public sealed class AddEducationCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IBaseRepository<Education> educationRepository)
    : IRequestHandler<AddEducationCommand, ErrorOr<ApiResponse<EducationRespone>>>
{
    public async Task<ErrorOr<ApiResponse<EducationRespone>>> Handle(
        AddEducationCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;

        if (currentUser == null || !currentUser.Identity?.IsAuthenticated == true)
        {
            return Error.Failure(
                code: "User.Failure",
                description: "User is not authenticated.");
        }

        var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roles = currentUser.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (!roles.Contains("Member"))
        {
            return Error.Forbidden(
                code: "User.Forbidden",
                description: "Access denied. Only members can add education.");
        }

        if (userId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "Access denied. The MemberId is invalid.");
        }

        var education = new Education
        {
            Id = Guid.NewGuid(),
            MemberId = userId,
            SchoolName = request.SchoolName,
            FieldOfStudy = request.FieldOfStudy,
            LevelOfEducation = request.LevelOfEducation
        };

        await unitOfWork.Educations.AddEntityAsync(education);
        await unitOfWork.CompleteAsync(cancellationToken);

        var educationResponse = new EducationRespone(
            education.Id,
            education.SchoolName,
            education.LevelOfEducation,
            education.FieldOfStudy);

        return new ApiResponse<EducationRespone>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Message = "Education added successfully.",
            Data = educationResponse
        };
    }
}