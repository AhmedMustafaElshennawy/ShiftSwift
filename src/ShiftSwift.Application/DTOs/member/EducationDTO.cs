namespace ShiftSwift.Application.DTOs.member
{
    public sealed record EducationDTO(
        string Institution, 
        string Degree);

    public sealed record EducationRespone(
        Guid Id,
        string MemberId,
        string Institution,
        string Degree);
}
