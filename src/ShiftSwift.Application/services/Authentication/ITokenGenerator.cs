using ShiftSwift.Domain.identity;


namespace ShiftSwift.Application.services.Authentication
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateToken(Company company, string Role);
        public Task<string> GenerateToken(Member member, string Role);
        public Task<string> GenerateToken(Account account, string Role);
    }
}
