using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;


namespace ShiftSwift.Application.Features.experience.Commands.AddExperience
{
    public sealed record AddExperienceCommand(string MemberId,
        string Title,
        string CompanyName,
        DateTime StartDate,
        DateTime? EndDate,
        string? Description):IRequest<ErrorOr<ApiResponse<AddExperienceResponse>>>;
   
}
