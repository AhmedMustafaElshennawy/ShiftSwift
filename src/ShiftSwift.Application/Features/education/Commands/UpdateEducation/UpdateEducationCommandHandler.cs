using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;
using System.Security.Claims;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Features.education.Commands.UpdateEducation;

internal sealed class UpdateEducationCommandHandler(
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IBaseRepository<Education> educationRepository)
    : IRequestHandler<UpdateEducationCommand, ErrorOr<ApiResponse<UpdateEducationRespone>>>
{
    public async Task<ErrorOr<ApiResponse<UpdateEducationRespone>>> Handle(
        UpdateEducationCommand request,
        CancellationToken cancellationToken)
    {
        var currentUser = httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.Identity?.IsAuthenticated == true)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
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
                description: "Access denied. Only members can update education.");
        }

        // 3. Validate member ID matches
        if (userId != request.MemberId)
        {
            return Error.Unauthorized(
                code: "User.Unauthorized",
                description: "You can only update your own education records.");
        }

        var education = await educationRepository.Entites()
            .FirstOrDefaultAsync(e => e.MemberId == request.MemberId, cancellationToken);

        if (education == null)
        {
            return Error.NotFound(
                code: "Education.NotFound",
                description: "Education record not found for this member.");
        }

        education.Level = request.Level;
        education.Faculty = request.Faculty;
        education.UniversityName = request.UniversityName;

        await unitOfWork.Educations.UpdateAsync(education);
        await unitOfWork.CompleteAsync(cancellationToken);

        var response = new UpdateEducationRespone(
            education.Id,
            education.Level,
            education.Faculty,
            education.UniversityName);

        return new ApiResponse<UpdateEducationRespone>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Message = "Education record updated successfully.",
            Data = response
        };
    }
}