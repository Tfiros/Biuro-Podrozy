using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TinProjektBackend.Models;
using TinProjektBackend.Models.RequestModels;
using TinProjektBackend.Models.ResponseModels;
using TinProjektBackend.Repositories;

namespace TinProjektBackend.Services;

public interface IUserService
{
    Task RegisterUserAsync(RegisterRequestModel request);
    Task<TokenResponseModel> LoginUserAsync(LoginRequestModel request);
    Task<TokenResponseModel> RefreshUserTokenAsync(RefreshTokenRequestModel refToken);

}
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    public IConfiguration _configuration { get; set; }

    public UserService(IUserRepository userRepository, 
        IUserRoleRepository userRoleRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _configuration = configuration;
    }

    public async Task RegisterUserAsync(RegisterRequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Wymagane login i haslo!");
        }

        if (await _userRepository.UserExistsByLoginAsync(request.Login))
        {
            throw new InvalidOperationException("Login zajety.");
        }

        var byteSalt = PasswordHasher.GenerateSalt();
        var salt = Convert.ToBase64String(byteSalt);
        var hashedPassword = PasswordHasher.HashPassword(request.Password, byteSalt);

        var user = new User
        {
            Login = request.Login,
            Password = hashedPassword,
            Salt = salt,
            RoleId = 2,
            RefreshToken = PasswordHasher.GenerateRefreshToken(),
            ExpirationDate = DateTime.Now.AddDays(1)
        };

        var client = new Client
        {
            User = user,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Address = request.Address
        };

        await _userRepository.RegisterUserAsync(user, client);
    }

    public async Task<TokenResponseModel> LoginUserAsync(LoginRequestModel request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("Login and password are required.");
        }
        var user = await _userRepository.GetUserByLoginAsync(request.Login);
        if (user == null)
        {
            throw new InvalidOperationException("Invalid login or password.");
        }

        var hashedPassword = PasswordHasher.GetHasedPasswordWithSalt(request.Password, user.Salt);
        if (hashedPassword != user.Password)
        {
            throw new InvalidOperationException("Invalid login or password.");
        }
        
        var role = await _userRoleRepository.GetRoleByIdAsync(user.RoleId);

        Claim[] userClaims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, role.Name)
        };
        
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        var refToken = await _userRepository.UpdateRefreshTokenAsync(user);
        return new TokenResponseModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refToken
        };
    }

    public async Task<TokenResponseModel> RefreshUserTokenAsync(RefreshTokenRequestModel refToken)
    {
        var user = await _userRepository.GetUserByRefreshTokenAsync(refToken.RefreshToken);
        
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.ExpirationDate < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }
        
        var role = await _userRoleRepository.GetRoleByIdAsync(user.RoleId);

        Claim[] userClaims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, role.Name)
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );
        
        var newRefToken = await _userRepository.UpdateRefreshTokenAsync(user);
        return new TokenResponseModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = newRefToken
        };
    }
}
