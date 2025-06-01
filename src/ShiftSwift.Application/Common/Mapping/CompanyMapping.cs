using Mapster;
using ShiftSwift.Application.Features.ProfileData.Commands.AddCompanyProfileData;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class CompanyMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Company, AddOrUpdateCompanyProfileInformationResponse>()
            .Map(dest => dest.CompanyId, src => src.Id)
            .Map(dest => dest.CompanyName, src => src.CompanyName);
    }
}