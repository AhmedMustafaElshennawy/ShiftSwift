using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.GetApplicants
{
    public sealed record GetApplicantsQuery(Guid JobId)
     : IRequest<ErrorOr<ApiResponse<IReadOnlyList<GetApplicantsResponse>>>>;

}
