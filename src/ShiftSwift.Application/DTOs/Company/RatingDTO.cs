namespace ShiftSwift.Application.DTOs.Company
{
    public sealed record RatingDTO(
        decimal Score,
        string? Comment);

    public sealed record RatingResponse(
        Guid Id,
        string CompanyId,
        string RatedById,
        decimal Score,
        string? Comment,
        DateTime CreatedAt);
    
    public sealed record CompanyRatingResponse(
    decimal Score,        
    string? Comment,       
    DateTime CreatedAt,    
    string RatedByUserName,
    string? RatedByImageUrl);

    public sealed record AverageRatingResponse(
        string CompanyId,
        decimal? AverageScore,
        List<CompanyRatingResponse>? Ratings

        );

}

