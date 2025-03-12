namespace ShiftSwift.Application.services.Authentication
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =true)]
    public class AuthorizaionAttribute:Attribute
    {
        public string? Roles { get; set; }
    }
}
