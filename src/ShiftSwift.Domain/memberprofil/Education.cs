using ShiftSwift.Domain.identity;


namespace ShiftSwift.Domain.models.memberprofil
{
    public class Education
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public required string LevelOfEducation { get; set; }
        public required string FieldOfStudy { get; set; }
        public required string SchoolName { get; set; }
    }
}
