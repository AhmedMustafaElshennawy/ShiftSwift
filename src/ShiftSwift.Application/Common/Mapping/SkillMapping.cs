using Mapster;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Common.Mapping;

internal class SkillMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Skill, MemberSkillResponse>();

    }
}