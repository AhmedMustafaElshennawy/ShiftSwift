using Mapster;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class ExperienceMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Experience, MemberExperienceInJobResponse>();

        config.NewConfig<Experience, MemberExperienceResponse>();

    }
}