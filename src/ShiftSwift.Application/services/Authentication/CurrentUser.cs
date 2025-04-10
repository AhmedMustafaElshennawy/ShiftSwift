namespace ShiftSwift.Application.services.Authentication
{
    public sealed record CurrentUser(string UserId, 
        string UserName, 
        string Email, 
        List<string> Roles);
    
}