using Microsoft.EntityFrameworkCore;
using TinProjektBackend.Context;
using TinProjektBackend.Models.ResponseModels;

namespace TinProjektBackend.Repositories;
public interface IUserRoleRepository
{
    Task<RoleResponseModel> GetRoleByIdAsync(int roleId);
}
public class UserRoleRepository : IUserRoleRepository
{
    private readonly DatabaseContext _context;

    public UserRoleRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<RoleResponseModel> GetRoleByIdAsync(int roleId)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        return new RoleResponseModel()
        {
            RoleId = userRole.RoleId,
            Name = userRole.RoleName
        };
    }
}