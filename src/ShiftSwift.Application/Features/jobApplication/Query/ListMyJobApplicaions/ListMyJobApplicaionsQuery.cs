using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.jobApplication.Query.ListMyJobApplicaions
{
    public sealed record ListMyJobApplicaionsQuery(string MemberId)
        :IRequest<ErrorOr<ApiResponse<IReadOnlyList<ListMyJobApplicaionsResponse>>>>;   
}
