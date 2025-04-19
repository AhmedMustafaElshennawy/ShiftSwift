using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Application.DTOs.Company;
using ShiftSwift.Domain.identity;
using ShiftSwift.Shared.ApiBaseResponse;
using System.Net;

namespace ShiftSwift.Application.Features.Authentication.Queries.GetCompanyInfo
{
    public sealed record GetCompanyInfoById(string Id):IRequest<ErrorOr<ApiResponse<CompanyResponseInfo>>>;

    public sealed class GetCompanyInfoByIdHandler : IRequestHandler<GetCompanyInfoById, ErrorOr<ApiResponse<CompanyResponseInfo>>>
    {
        private readonly IBaseRepository<Company> _repository;
        private readonly UserManager<Account> _userManager;
        public GetCompanyInfoByIdHandler(IBaseRepository<Company> repository, UserManager<Account> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public async Task<ErrorOr<ApiResponse<CompanyResponseInfo>>> Handle(GetCompanyInfoById request, CancellationToken cancellationToken)
        {

            var company = await _repository.Entites()
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (company is null)
            {
                return Error.NotFound("User.NotFound", "User not found.");
            }

            var isCompany = await _userManager.IsInRoleAsync(company, "Company");
            if (!isCompany)
            {
                return Error.NotFound(
                    code: "Company.NotFound",
                    description: "User is not a member.");
            }

            var response = new CompanyResponseInfo(
                CompanyId: company.Id,
                CompanyName: company.CompanyName,
                UserName: company.UserName!,
                PhoneNumber: company.PhoneNumber!,
                Email: company.Email!,
                Description: company.Description!
);

            return new ApiResponse<CompanyResponseInfo>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Message = "User profile retrieved successfully.",
                Data = response
            };
        }
    }
}
