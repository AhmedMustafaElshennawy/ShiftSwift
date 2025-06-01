using ShiftSwift.Domain.Enums;
using ShiftSwift.Domain.shared;

namespace ShiftSwift.Domain.Shared;

public class JobQuestion
{
    public Guid Id { get; set; }  

    public required string QuestionText { get; set; }

    public QuestionType QuestionType { get; set; }

    public Guid JobId { get; set; }

    public Job Job { get; set; } = null!;

    public ICollection<ApplicationAnswer> Answers { get; set; } = new List<ApplicationAnswer>();
}