using ErrorOr;
using MediatR;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.experience.Commands.DeleteEducation
{
    public sealed record DeleteExperienceCommand(string MemberId):IRequest<ErrorOr<ApiResponse<Deleted>>>;
    
}
