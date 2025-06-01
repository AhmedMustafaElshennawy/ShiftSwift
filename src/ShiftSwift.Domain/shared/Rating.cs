using ShiftSwift.Domain.identity;

namespace ShiftSwift.Domain.shared;

public class Rating
{
    public Guid Id { get; set; }
    public string RatedById { get; set; }
    public string CompanyId { get; set; }
    public decimal Score { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public Member RatedBy { get; set; }
    public Company Company { get; set; }
}