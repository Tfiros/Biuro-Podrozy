using Microsoft.EntityFrameworkCore;
using TinProjektBackend.Models;

namespace TinProjektBackend.Context;

public class DatabaseContext : Microsoft.EntityFrameworkCore.DbContext
{
    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripCategory> TripCategories { get; set; }

    public virtual DbSet<TripSchedule> TripSchedules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
