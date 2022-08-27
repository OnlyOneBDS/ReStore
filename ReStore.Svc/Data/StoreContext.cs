using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.Entities;
using ReStore.Svc.Entities.OrderAggregate;

namespace ReStore.Svc.Data
{
  public class StoreContext : IdentityDbContext<User, Role, int>
  {
    public StoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<User>()
             .HasOne(a => a.Address)
             .WithOne()
             .HasForeignKey<UserAddress>(a => a.Id)
             .OnDelete(DeleteBehavior.Cascade);

      builder.Entity<Role>()
             .HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = 2, Name = "Member", NormalizedName = "MEMBER" }
              );
    }
  }
}