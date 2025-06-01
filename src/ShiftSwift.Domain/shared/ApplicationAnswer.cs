using ShiftSwift.Domain.shared;

namespace ShiftSwift.Domain.Shared;

public class ApplicationAnswer
{
    public Guid Id { get; set; }
    public Guid JobApplicationId { get; set; }
    public JobApplication JobApplication { get; set; } = null!;

    public Guid JobQuestionId { get; set; }
    public JobQuestion JobQuestion { get; set; } = null!;

    public string? AnswerText { get; set; }

    public bool? AnswerBool { get; set; }
}