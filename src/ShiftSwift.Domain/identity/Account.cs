using Microsoft.AspNetCore.Identity;


namespace ShiftSwift.Domain.identity
{
    public class Account : IdentityUser<string>
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
}