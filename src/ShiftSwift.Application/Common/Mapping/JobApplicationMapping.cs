using Mapster;
using ShiftSwift.Application.Features.jobApplication.Command.CreateJobApplication;
using ShiftSwift.Application.Features.jobApplication.Query.GetLastWorkJobsForMember;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class JobApplicationMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<JobApplication, AddJobApplicationResponse>();

        config.NewConfig<JobApplication, GetLastWorkJobResponse>();

    }
}