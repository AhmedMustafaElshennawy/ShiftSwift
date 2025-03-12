namespace ShiftSwift.Application.services.Authentication
{
    public record CurrentUser(string UserId, 
        string UserName, 
        string Email, 
        List<string> Roles);
    
}
