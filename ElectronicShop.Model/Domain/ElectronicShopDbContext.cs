using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Model.Domain
{
    public class ElectronicShopDbContext : IdentityDbContext<Users, Role, string>
    {
        public ElectronicShopDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(users =>
            {
                users.HasMany(x => x.Claims)
                     .WithOne()
                     .HasForeignKey(x => x.UserId)
                     .IsRequired()
                     .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityUserRole<string>>();
            modelBuilder.Entity<IdentityUserClaim<string>>();
            modelBuilder.Entity<IdentityRoleClaim<string>>();
            modelBuilder.Entity<Role>(roles =>
            {
                roles.HasMany(x => x.Claims)
                .WithOne()
                .HasForeignKey(x => x.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}