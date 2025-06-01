using ShiftSwift.Domain.shared;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShiftSwift.Domain.identity;

public class Company : Account
{
    [NotMapped]
    public string CompanyName => $"{FirstName} {LastName}";
    public string? Field { get; set; }
    public string? Overview { get; set; }
    public DateTime? DateOfEstablish { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Area { get; set; }
    public double? Rating { get; set; }
    public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    public ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();
}