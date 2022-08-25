using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.Entities;

namespace ReStore.Svc.Data
{
  public class StoreContext : IdentityDbContext<User>
  {
    public StoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder.Entity<IdentityRole>()
             .HasData(
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "Member", NormalizedName = "MEMBER" }
              );
    }
  }
}