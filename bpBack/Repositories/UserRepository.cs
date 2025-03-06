using Microsoft.EntityFrameworkCore;
using TinProjektBackend.Context;
using TinProjektBackend.Models;

namespace TinProjektBackend.Repositories;


public interface IUserRepository
{
    Task<User> GetUserByLoginAsync(string login);
    Task<bool> UserExistsByLoginAsync(string login);
    Task<int> CreateUserAsync(User user);
    Task CreateClientAsync(Client client);
    Task RegisterUserAsync(User user, Client client);
    Task<string> UpdateRefreshTokenAsync(User user);
    Task<User> GetUserByRefreshTokenAsync(string refToken);
    Task<bool> UserExistsByIdAsybc(int id);
}

public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByLoginAsync(string login)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<bool> UserExistsByLoginAsync(string login)
    {
        return await _context.Users.AnyAsync(u => u.Login == login);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }

    public async Task CreateClientAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
    }

    public async Task RegisterUserAsync(User user, Client client)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                await CreateUserAsync(user);
                await CreateClientAsync(client);
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
    public async Task<string> UpdateRefreshTokenAsync(User user)
    {
        user.RefreshToken = PasswordHasher.GenerateRefreshToken();
        user.ExpirationDate = DateTime.Now.AddDays(1);

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user.RefreshToken;
    }

    public async Task<User> GetUserByRefreshTokenAsync(string refToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.RefreshToken == refToken);
    }

    public async Task<bool> UserExistsByIdAsybc(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(t => t.UserId == id);
        if (user is  null)
        {
            return false;
        }

        return true;
    }
}

