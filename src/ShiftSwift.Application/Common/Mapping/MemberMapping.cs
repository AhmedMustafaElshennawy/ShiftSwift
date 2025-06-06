using Mapster;
using ShiftSwift.Application.DTOs.member;
using ShiftSwift.Application.Features.Authentication.Commands.Registermamber;
using ShiftSwift.Application.Features.Authentication.Queries.GetCurrentUserInformation;
using ShiftSwift.Application.Features.Authentication.Queries.LogInMember;
using ShiftSwift.Application.Features.jobApplication.Query.GetMyLastWorkApplicants;
using ShiftSwift.Application.Features.jobApplication.Query.GetSpecificApplicant;
using ShiftSwift.Application.Features.ProfileData.Commands.AddMemberProfileData;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class MemberMapping:IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Member, GetMyLastWorkApplicantsResponse>()
            .Map(dest => dest.MemberId, src => src.Id);

        config.NewConfig<Member, SpecificApplicantForSpecificJobResponse>()
            .Map(dest => dest.MemberId, src => src.Id);

        TypeAdapterConfig<Member, AddMemberProfileDataResponse>.NewConfig()
            .Map(dest => dest.MemberId, src => src.Id)
            .Map(dest => dest.DateOfBirth, src => src.BirthDate);

        config.NewConfig<Member, RegisterMemberResponse>()
            .Map(dest => dest.MemberId, src => src.Id);

        config.NewConfig<Member, LoginMemberResponse>()
            .Map(dest => dest.MemberId, src => src.Id);

        config.NewConfig<Member, CurrentMemberResponse>()
            .Map(dest => dest.MemberId, src => src.Id)
            .Map(dest => dest.DateOfBirth, src => src.BirthDate);

        config.NewConfig<Member, MemberResponseInfo>()
            .Map(dest => dest.MemberId, src => src.Id)
            .Map(dest => dest.DateOfBirth, src => src.BirthDate);

    }
}