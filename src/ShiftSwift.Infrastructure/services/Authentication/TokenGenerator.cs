using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ShiftSwift.Infrastructure.services.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings _jwt;
        private readonly UserManager<Account> _userManager;
        public TokenGenerator(IOptions<JwtSettings> jwt, UserManager<Account> userManager)
        {
            _jwt = jwt.Value;
            _userManager = userManager;
        }

        //public async Task<string> GenerateToken(Company User, string Role)
        //{
        //    var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var UserRoles = await _userManager.GetRolesAsync(User);
        //    var RoleClaims = UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, User.Id),
        //        new Claim(ClaimTypes.Name, User.UserName!),
        //        new Claim(JwtRegisteredClaimNames.Email, User.Email!),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    }.Union(RoleClaims); // Add roles as ==> claims

        //    var credentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(
        //        issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(_jwt.DurationInDays),
        //        signingCredentials: credentials);

        //    var response = new JwtSecurityTokenHandler().WriteToken(token);
        //    return response;
        //}

        //public async Task<string> GenerateToken(Member User, string Role)
        //{
        //    var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        //    var UserRoles = await _userManager.GetRolesAsync(User);
        //    var RoleClaims = UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, User.Id),
        //        new Claim(ClaimTypes.Name, User.UserName!),
        //        new Claim(JwtRegisteredClaimNames.Email, User.Email!),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    }.Union(RoleClaims); // Add roles as ==> claims

        //    var credentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(
        //        issuer: _jwt.Issuer,
        //        audience: _jwt.Audience,
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(_jwt.DurationInDays),
        //        signingCredentials: credentials);

        //    var response = new JwtSecurityTokenHandler().WriteToken(token);
        //    return response;
        //}
        public async Task<string> GenerateToken(Account account, string Role)
        {
            var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var UserRoles = await _userManager.GetRolesAsync(account);
            var RoleClaims = UserRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(ClaimTypes.Name, account.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, account.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Union(RoleClaims); // Add roles as ==> claims

            var credentials = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
