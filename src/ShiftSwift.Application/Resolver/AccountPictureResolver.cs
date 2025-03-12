using Microsoft.Extensions.Configuration;
using ShiftSwift.Domain.identity;

namespace ShiftSwift.Application.Resolver
{
    public sealed class AccountPictureResolver
    {
        private readonly IConfiguration _configuration;
        public AccountPictureResolver(IConfiguration configuration) => _configuration = configuration;
        public string Resolve(Account account)
        {
            if (account is null || string.IsNullOrEmpty(account.ImageUrl))
                return string.Empty;

            return $"{_configuration["ApiBaseUrl"]}/api/Account/GetProfilePicture/{account.ImageUrl}";
        }
    }
}