namespace ShiftSwift.Application.DTOs.member
{
    public sealed record EducationDto(
        string Level,
        string Faculty,
        string UniversityName);

    public sealed record EducationRespone(
        Guid Id,
        string Level,
        string Faculty,
        string UniversityName);
}
