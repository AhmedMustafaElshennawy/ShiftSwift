using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.models.memberprofil;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.skill.Commands.DeleteSkill
{
    public sealed class DeleteSkillCommandHandler : IRequestHandler<DeleteSkillCommand, ErrorOr<ApiResponse<Deleted>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IBaseRepository<Skill> _skillRepository;

        public DeleteSkillCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserProvider currentUserProvider,
            IBaseRepository<Skill> skillRepository)
        {
            _unitOfWork = unitOfWork;
            _currentUserProvider = currentUserProvider;
            _skillRepository = skillRepository;
        }

        public async Task<ErrorOr<ApiResponse<Deleted>>> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
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
                    description: "Access denied. Only members can delete skills."
                );
            }

            if (currentUser.UserId != request.MemberId)
            {
                return Error.Unauthorized(
                    code: "User.Unauthorized",
                    description: $"Access denied. The MemberId You Entered Is Wrong {request.MemberId}"
                );
            }

            var currentUserSkill = await _skillRepository.Entites()
             .Where(x => x.MemberId == currentUser.UserId)
               .FirstOrDefaultAsync(cancellationToken);


            if (currentUserSkill is null)
            {
                return Error.NotFound(
                    code: "Skill.NotFound",
                    description: "No skill found for the current user."
                );
            }

            var deletionResult = await _unitOfWork.Skills.DeleteAsync(currentUserSkill);
            if (!deletionResult)
            {
                return Error.Failure(
                    code: "Skill.Failure",
                    description: "Failed to delete skill for the current user."
                );
            }

            await _unitOfWork.CompleteAsync(cancellationToken);
            return new ApiResponse<Deleted>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.NoContent,
                Message = "Skill deleted successfully.",
                Data = null
            };

        }
    }
}
