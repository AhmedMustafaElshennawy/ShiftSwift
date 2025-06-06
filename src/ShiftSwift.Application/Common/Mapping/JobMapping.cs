using Mapster;
using ShiftSwift.Application.Features.job.Commands.PostJob;
using ShiftSwift.Application.Features.job.Commands.UpdatePostJob;
using ShiftSwift.Application.Features.job.Queries.GetAllJobPosts;
using ShiftSwift.Application.Features.job.Queries.GetJobPostById;
using ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class JobMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Job, CompanyPostedJobResponse>()
            .Map(dest => dest.JobId, src => src.Id)
            .Map(dest => dest.JobType, src => src.JobTypeId)
            .Map(dest => dest.WorkMode, src => src.WorkModeId)
            .Map(dest => dest.SalaryType, src => src.SalaryTypeId);


        config.NewConfig<Job, UpdatePostedJobResponse>()
            .Map(dest => dest.JobId, src => src.Id)
            .Map(dest => dest.JobType, src => src.JobTypeId)
            .Map(dest => dest.WorkMode, src => src.WorkModeId)
            .Map(dest => dest.SalaryType, src => src.SalaryTypeId);

        config.NewConfig<Job, PostedJobByIdResponse>()
            .Map(dest => dest.JobId, src => src.Id)
            .Map(dest => dest.JobType, src => src.JobTypeId)
            .Map(dest => dest.WorkMode, src => src.WorkModeId)
            .Map(dest => dest.SalaryType, src => src.SalaryTypeId);

        config.NewConfig<Job, GetAllPostedJobForSpecificCompanyResponse>()
            .Map(dest => dest.JobId, src => src.Id)
            .Map(dest => dest.JobType, src => src.JobTypeId)
            .Map(dest => dest.WorkMode, src => src.WorkModeId)
            .Map(dest => dest.SalaryType, src => src.SalaryTypeId);

    }
}