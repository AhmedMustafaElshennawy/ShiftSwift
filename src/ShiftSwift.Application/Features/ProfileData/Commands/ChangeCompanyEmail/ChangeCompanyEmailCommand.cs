using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.ChangeCompanyEmail
{

    public sealed record ChangeCompanyEmailCommand(string CompanyId, string Email)
        : IRequest<ErrorOr<ApiResponse<CompanyResponse>>>;
}
