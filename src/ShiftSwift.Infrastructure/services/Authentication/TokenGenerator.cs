using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShiftSwift.Application.services.Authentication;
using ShiftSwift.Domain.identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ShiftSwift.Infrastructure.services.Authentication;

public sealed class TokenGenerator(IOptions<JwtSettings> jwt, UserManager<Account> userManager)
    : ITokenGenerator
{
    private readonly JwtSettings _jwt = jwt.Value;

    public async Task<string> GenerateToken(Company user, string Role)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var userRoles = await userManager.GetRolesAsync(user);
        var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }.Union(roleClaims); // Add roles as ==> claims

        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: credentials);

        var response = new JwtSecurityTokenHandler().WriteToken(token);
        return response;
    }

    public async Task<string> GenerateToken(Member user, string Role)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var userRoles = await userManager.GetRolesAsync(user);
        var roleClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }.Union(roleClaims); // Add roles as ==> claims

        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: credentials);

        var response = new JwtSecurityTokenHandler().WriteToken(token);
        return response;
    }
}