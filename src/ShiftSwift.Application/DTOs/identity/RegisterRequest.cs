namespace ShiftSwift.Application.DTOs.identity
{
    public sealed record RegisterMemberRequest(
        string Email,
        string UserName,
        string Password,
        string PhoneNumber);

    public sealed record RegisterCompanyRequest(
        string Email,
        string UserName,
        string Password,
        string PhoneNumber);
}
