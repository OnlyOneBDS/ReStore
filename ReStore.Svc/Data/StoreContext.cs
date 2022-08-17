using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.Entities;

namespace ReStore.Svc.Data
{
  public class StoreContext : DbContext
  {
    public StoreContext(DbContextOptions options) : base(options) { }

    public DbSet<Product> Products { get; set; }
  }
}