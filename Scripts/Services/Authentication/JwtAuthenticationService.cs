using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace SmartFeedback.Scripts.Services.Authentication;

public static class JwtAuthenticationService
{
    public static string GenerateToken(string userId)
    {
        Debug.Assert(AppSettings.Authorization != null, "AppSettings.Authorization != null");
        var credentials = new SigningCredentials(AppSettings.Authorization.GetKey, SecurityAlgorithms.HmacSha256);
        
        var securityToken = new JwtSecurityToken(
            AppSettings.Authorization.Issuer,
            AppSettings.Authorization.Audience,
            claims: new[]
            {
                new Claim(ClaimTypes.Sid, userId)
            },
            expires: DateTime.UtcNow.AddMonths(3),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private static string? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.ReadToken(token) is not JwtSecurityToken jwtToken)
            return null;

        var claims = jwtToken.Claims;

        var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        var userId = userIdClaim?.Value;

        return userId;
    }
    
    public static string? GetUserIdAsync(HttpRequest request)
    {
        var token = request.Headers.Authorization.ToString().Replace("Bearer ", "");
        return token == string.Empty ? null : GetUserIdFromToken(token);
    }
}
