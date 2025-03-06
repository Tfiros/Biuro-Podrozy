using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

public class PasswordHasher
{

    public static byte[] GenerateSalt()
    {
        var rng = new RNGCryptoServiceProvider();
        var saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        return saltBytes;
    }

    public static string HashPassword(string password, byte[] salt)
    {
        string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 5000,
            numBytesRequested: 256/8
            ));
        return hashedPassword;
    }

    public static string GetHasedPasswordWithSalt(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        
        string currHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 5000,
            numBytesRequested: 256/8
        ));
        return currHashedPassword;
    }

    public static string GenerateRefreshToken()
    {
        var randNum = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randNum);
            return Convert.ToBase64String(randNum);
        }
    }
    
    public static string GetUserIdFromAccessToken(string accessToken, string secret, IConfiguration config)
    {
        var tokenValidationParamters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateActor = true,
            ClockSkew = TimeSpan.FromMinutes(2),
            ValidIssuer = config["JWT:Issuer"],
            ValidAudience = config["JWT:Audience"],
            ValidateLifetime = false,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secret)
                )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParamters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token!");
        }

        var userId = principal.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new SecurityTokenException($"Missing claim: {ClaimTypes.Name}!");
        }

        return userId;
    }
}