using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Service.Contracts;
using ECommerce.Service.Contracts.Extensions;
using ECommerce.Service.Services.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ECommerce.Service.Services.Utilities.Security.JWT;

public class JwtHelper : ITokenHelper
{
    private readonly TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
    }

    public string GenerateAccessToken(long personId, string role, List<RoleClaimExtDto> roleClaimExtDtos)
    {
        _accessTokenExpiration = DateTime.UtcNow.AddSeconds(_tokenOptions.AccessTokenExpiration);
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        var jwt = CreateJwtSecurityToken(_tokenOptions, personId, role, signingCredentials, roleClaimExtDtos);
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.WriteToken(jwt);
        return accessToken;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public JwtSecurityToken CreateJwtSecurityToken(
        TokenOptions tokenOptions,
        long personId,
        string role,
        SigningCredentials signingCredentials,
        List<RoleClaimExtDto> roleClaimExtDtos
    )
    {
        var jwt = new JwtSecurityToken(
            issuer: tokenOptions.Issuer,
            audience: tokenOptions.Audience,
            expires: _accessTokenExpiration,
            notBefore: DateTime.UtcNow,
            claims: SetOperationClaims(personId, role, roleClaimExtDtos),
            signingCredentials: signingCredentials
        );
        return jwt;
    }

    private List<Claim> SetOperationClaims(long personId, string role, IEnumerable<RoleClaimExtDto> roleClaimExtDtos)
    {
        var claims = new List<Claim>();
        claims.AddPersonId(personId.ToString());
        claims.AddRoleClaims(roleClaimExtDtos.Select(r => r.ClaimTitle).ToArray());
        claims.AddRole(role);

        return claims;
    }

    public IResponse GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwt || !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                StringComparison.InvariantCultureIgnoreCase))
        {
            return new ErrorResponse(Messages.Authorization_CanNotGetClaimsPrincipal);
        }

        return new SuccessDataResponse<ClaimsPrincipal>(principal);
    }
}