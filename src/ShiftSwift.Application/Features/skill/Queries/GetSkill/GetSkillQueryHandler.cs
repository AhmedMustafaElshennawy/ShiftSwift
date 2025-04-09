using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.skill.Queries.GetSkill
{
    public sealed class GetSkillQueryHandler : IRequestHandler<GetSkillQuery, ErrorOr<ApiResponse<SkillResponse>>>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Skill> _skillRepository;

        public GetSkillQueryHandler(ICurrentUserProvider currentUserProvider, IBaseRepository<Skill> skillRepository)
        {
            _currentUserProvider = currentUserProvider;
            _skillRepository = skillRepository;
        }

        public async Task<ErrorOr<ApiResponse<SkillResponse>>> Handle(GetSkillQuery request, CancellationToken cancellationToken)
        {
            var currentUserResult = await _currentUserProvider.GetCurrentUser();
            if (currentUserResult.IsError)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: currentUserResult.Errors.FirstOrDefault().Description ?? "User is not authenticated.");
            }

            var currentUser = currentUserResult.Value;
            if (!currentUser.Roles.Contains("Member"))
            {
                return Error.Forbidden(
                    code: "User.Forbidden",
                    description: "Access denied. Only members can view skills.");
            }

            if (request.MemberId != currentUser.UserId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            var currentUserSkill = await _skillRepository.Entites()
                .Where(x => x.MemberId == currentUser.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (currentUserSkill is null)
            {
                return new ApiResponse<SkillResponse>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Skill Retrieved successfully.",
                    Data = "The Current User Has No Skills."
                };
            }

            var skillResponse = new SkillResponse(
                currentUser.UserId,
                currentUserSkill.Name
            );

            return new ApiResponse<SkillResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "Skill Retrieved successfully.",
                Data = skillResponse
            };
        }
    }
}
