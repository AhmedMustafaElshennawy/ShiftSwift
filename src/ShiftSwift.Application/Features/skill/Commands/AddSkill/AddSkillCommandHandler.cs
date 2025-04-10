using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.skill.Commands.AddSkill
{
    public sealed class AddSkillCommandHandler : IRequestHandler<AddSkillCommand, ErrorOr<ApiResponse<SkillResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Skill> _skillRepository;

        public AddSkillCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Skill> skillRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _skillRepository = skillRepository;
        }

        public async Task<ErrorOr<ApiResponse<SkillResponse>>> Handle(AddSkillCommand request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only members can add skills.");
            }

            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}");
            }

            var currentUserSkill = await _skillRepository.Entites()
                .Where(x => x.MemberId == currentUser.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            bool isUpdated = currentUserSkill is not null;
            if (isUpdated)
            {
                currentUserSkill!.Name = request.Name;
                await _unitOfWork.Skills.UpdateAsync(currentUserSkill);
            }
            else
            {
                var newSkill = new Skill
                {
                    Id = Guid.NewGuid(),
                    MemberId = currentUser.UserId,
                    Name = request.Name
                };
                await _unitOfWork.Skills.AddEntityAsync(newSkill);
            }

            await _unitOfWork.CompleteAsync(cancellationToken);

            var response = new SkillResponse(
                currentUser.UserId,
                request.Name);

            return new ApiResponse<SkillResponse>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = isUpdated ? "Skill updated successfully." : "Skill added successfully.",
                Data = response
            };
        }
    }
}
