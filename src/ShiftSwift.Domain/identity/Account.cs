using Microsoft.AspNetCore.Identity;


namespace ShiftSwift.Domain.identity;

public class Account : IdentityUser<string>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
}