namespace ShiftSwift.Application.DTOs.member
{
    public sealed record EducationDTO(
        string SchoolName,
        string LevelOfEducation,
        string FieldOfStudy);

    public sealed record EducationRespone(
        Guid Id,
        string SchoolName,
        string LevelOfEducation,
        string FieldOfStudy);
}
