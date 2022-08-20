using Microsoft.EntityFrameworkCore;
using ReStore.Svc.Entities;

namespace ReStore.Svc.Data
{
  public class StoreContext : DbContext
  {
    public StoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Product> Products { get; set; }
  }
}