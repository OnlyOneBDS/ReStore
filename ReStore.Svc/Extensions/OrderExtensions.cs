using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.DTOs;
using ReStore.Svc.Entities.OrderAggregate;

namespace ReStore.Svc.Extensions
{
  public static class OrderExtensions
  {
    public static IQueryable<OrderDto> ProjectOrderToOrderDto(this IQueryable<Order> query)
    {
      return query.Select(order => new OrderDto
      {
        Id = order.Id,
        BuyerId = order.BuyerId,
        OrderDate = order.OrderDate,
        ShippingAddress = order.ShippingAddress,
        Subtotal = order.Subtotal,
        DeliveryFee = order.DeliveryFee,
        Total = order.GetTotal(),
        OrderStatus = order.OrderStatus.ToString(),
        OrderItems = order.OrderItems
                          .Select(item => new OrderItemDto
                          {
                            ProductId = item.ItemOrdered.ProductId,
                            Name = item.ItemOrdered.Name,
                            ImageUrl = item.ItemOrdered.ImageUrl,
                            Price = item.Price,
                            Quantity = item.Quantity
                          }).ToList()
      }).AsNoTracking();
    }
  }
}