using Mapster;
using ShiftSwift.Application.Features.Authentication.Commands.RegisterCompany;
using ShiftSwift.Application.Features.Authentication.Queries.GetCompanyInfo;
using ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserInformation;
using ShiftSwift.Application.Features.Authentication.Queries.LogInCompany;
using ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class CompanyMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Company, AddOrUpdateCompanyProfileInformationResponse>()
            .Map(dest => dest.CompanyId, src => src.Id);

        config.NewConfig<Company, CompanyResponse>()
            .Map(dest => dest.CompanyId, src => src.Id);

        config.NewConfig<Company, CompanyInformationByIdResponse>()
            .Map(dest => dest.CompanyId, src => src.Id);

        config.NewConfig<Company, LoginCompanyResponse>()
            .Map(dest => dest.CompanyId, src => src.Id);

        config.NewConfig<Company, CurrentCompanyResponse>()
            .Map(dest => dest.CompanyId, src => src.Id);

    }
}