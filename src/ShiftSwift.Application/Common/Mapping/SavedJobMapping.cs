using Mapster;
using ShiftSwift.Application.Features.savedJobs.Queries.GetSavedJobs;
using ShiftSwift.Domain.memberprofil;

namespace ShiftSwift.Application.Common.Mapping;

public sealed class SavedJobMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SavedJob, GetAllSavedJobsResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.JobId, src => src.JobId)
            .Map(dest => dest.SavedOn, src => src.SavedOn)
            .Map(dest => dest.JobTitle, src => src.Job.Title)
            .Map(dest => dest.CompanyId, src => src.Job.CompanyId)
            .Map(dest => dest.Title, src => src.Job.Title)
            .Map(dest => dest.Description, src => src.Job.Description)
            .Map(dest => dest.Location, src => src.Job.Location)
            .Map(dest => dest.PostedOn, src => src.Job.PostedOn)
            .Map(dest => dest.SalaryTypeId, src => src.Job.SalaryTypeId)
            .Map(dest => dest.Salary, src => src.Job.Salary)
            .Map(dest => dest.JobTypeTd, src => src.Job.JobTypeId);

    }
}