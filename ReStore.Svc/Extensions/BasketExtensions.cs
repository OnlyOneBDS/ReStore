using System.Linq;
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
  }
}