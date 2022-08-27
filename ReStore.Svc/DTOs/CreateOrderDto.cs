using ReStore.Svc.Entities.OrderAggregate;

namespace ReStore.Svc.DTOs
{
  public class CreateOrderDto
  {
    public bool SaveAddress { get; set; }
    public ShippingAddress ShippingAddress { get; set; }
  }
}