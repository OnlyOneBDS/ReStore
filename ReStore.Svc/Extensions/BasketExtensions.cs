using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.DTOs;
using ReStore.Svc.Entities;

namespace ReStore.Svc.Extensions
{
  public static class BasketExtensions
  {
    public static BasketDto MapBasketToDto(this Basket basket)
    {
      return new BasketDto
      {
        Id = basket.Id,
        BuyerId = basket.BuyerId,
        PaymentIntentId = basket.PaymentIntentId,
        ClientSecret = basket.ClientSecret,
        Items = basket.Items.Select(item => new BasketItemDto
        {
          ProductId = item.ProductId,
          Name = item.Product.Name,
          Price = item.Product.Price,
          ImageUrl = item.Product.ImageUrl,
          Brand = item.Product.Brand,
          Type = item.Product.Type,
          Quantity = item.Quantity
        }).ToList()
      };
    }

    public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
    {
      return query.Include(i => i.Items)
                  .ThenInclude(p => p.Product)
                  .Where(b => b.BuyerId == buyerId);
    }
  }
}