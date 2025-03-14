namespace ShiftSwift.Application.DTOs.Company
{
    public record GetRandomJobsResponse(Guid Id,
        string CompanyId, 
        string CompanyName, 
        string Title,
        string Description,
        string Location,
        DateTime PostedOn);
}
