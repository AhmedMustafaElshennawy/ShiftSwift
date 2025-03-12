namespace ShiftSwift.Domain.shared
{
    public class Rating 
    {
        public Guid Id { get; set; }
        public string RatedById { get; set; }
        public string CompanyId { get; set; }
        public double Score { get; set; }
        public DateTime CreatedAt {  get; set; }
    }
}
