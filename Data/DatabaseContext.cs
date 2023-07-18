using Microsoft.EntityFrameworkCore;
using RolesWithoutIdentity.Models;

namespace RolesWithoutIdentity.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(r => r.Role).WithMany(u => u.Users).HasForeignKey(fk => fk.RoleId);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r=>r.Id);
            });
        }

    }
}
