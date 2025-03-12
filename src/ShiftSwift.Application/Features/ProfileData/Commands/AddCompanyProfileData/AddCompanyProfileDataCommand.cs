using ErrorOr;
using MediatR;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Shared.ApiBaseResponse;

namespace ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData
{
    public sealed record AddCompanyProfileDataCommand(string CompanyId,
       string CompanyName,
       string Description) : IRequest<ErrorOr<ApiResponse<CompanyResponse>>>;
}