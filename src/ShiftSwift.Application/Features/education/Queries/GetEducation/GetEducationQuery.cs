using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.education.Queries.GetEducation
{
    public sealed record GetEducationQuery(string MemberId) :IRequest<ErrorOr<ApiResponse<EducationRespone>>>;
    
}
