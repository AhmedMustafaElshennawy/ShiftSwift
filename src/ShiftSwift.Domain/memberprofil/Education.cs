using ShiftSwift.Domain.identity;


namespace ShiftSwift.Domain.models.memberprofil
{
    public class Education
    {
        public Guid Id { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
    }
}
