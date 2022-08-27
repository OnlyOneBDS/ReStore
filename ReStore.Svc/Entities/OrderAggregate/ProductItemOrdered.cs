using Microsoft.EntityFrameworkCore;

namespace ReStore.Svc.Entities.OrderAggregate
{
  [Owned]
  public class ProductItemOrdered
  {
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
  }
}