using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartCookbook.Application.Services.Token;

public class TokenController
{
    private const string EmailAlias = "eml";
    private readonly double _expirationTimeInMinutes;
    private readonly string _securityKey;

    public TokenController(double expirationTimeInMinutes, string securityKey)
    {
        _expirationTimeInMinutes = expirationTimeInMinutes;
        _securityKey = securityKey;
    }

    public string TokenGen(string userEmail)
    {
        var claims = new List<Claim>
        {
            new Claim(EmailAlias, userEmail)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(SimetricKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParams = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            IssuerSigningKey = SimetricKey(),
            ClockSkew = new TimeSpan(0),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        var claims = tokenHandler.ValidateToken(token, validationParams, out _);

        return claims;
    }

    public string GetEmail(string token)
    {
        var claims = ValidateToken(token);

        return claims.FindFirst(EmailAlias).Value;
    }

    private SymmetricSecurityKey SimetricKey()
    {
        var symmetricKey = Convert.FromBase64String(_securityKey);
        return new SymmetricSecurityKey(symmetricKey);
    }
}
