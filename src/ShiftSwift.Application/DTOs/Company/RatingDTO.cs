namespace ShiftSwift.Application.DTOs.Company
{
    public sealed record RatingDTO(
        decimal Score);

    public sealed record RatingResponse(
        Guid Id,
        string CompanyId,
        string RatedById,
        decimal Score,
        DateTime CreatedAt);

    public sealed record AverageRatingResponse(
        string CompanyId,
        decimal? AverageScore);
}

