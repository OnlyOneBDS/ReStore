using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.Data;
using ReStore.Svc.DTOs;
using ReStore.Svc.Entities;

namespace ReStore.Svc.Controllers
{
  public class BasketController : BaseApiController
  {
    private readonly StoreContext _context;

    public BasketController(StoreContext context)
    {
      _context = context;
    }

    [HttpGet(Name = "GetBasket")]
    public async Task<ActionResult<BasketDto>> GetBasket()
    {
      var basket = await RetrieveBasket();

      return basket == null ? NotFound() : MapBasketToDto(basket);
    }

    [HttpPost]
    public async Task<ActionResult> AddItemToBasket(int productId, int quantity)
    {
      // Get basket
      var basket = await RetrieveBasket();

      if (basket == null)
      {
        // Create a new basket
        basket = CreateBasket();
      }

      // Get product
      var product = await _context.Products.FindAsync(productId);

      if (product == null)
      {
        return NotFound();
      }

      // Add item and quantity
      basket.AddItem(product, quantity);

      // Save changes
      var result = await _context.SaveChangesAsync() > 0;

      return result ? CreatedAtRoute("GetBasket", MapBasketToDto(basket)) : BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
    {
      // Get basket
      var basket = await RetrieveBasket();

      if (basket == null)
      {
        return NotFound();
      }

      // Remove item or reduce quantity
      basket.RemoveItem(productId, quantity);

      // Save changes
      var result = await _context.SaveChangesAsync() > 0;

      return result ? Ok() : BadRequest(new ProblemDetails { Title = "Problem removing item from basket" });
    }

    private Basket CreateBasket()
    {
      var buyerId = Guid.NewGuid().ToString();

      var cookieOptions = new CookieOptions
      {
        IsEssential = true,
        Expires = DateTime.Now.AddDays(30)
      };

      Response.Cookies.Append("buyerId", buyerId, cookieOptions);

      var basket = new Basket { BuyerId = buyerId };

      _context.Add(basket);

      return basket;
    }

    private async Task<Basket> RetrieveBasket()
    {
      return await _context.Baskets
        .Include(i => i.Items)
        .ThenInclude(p => p.Product)
        .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
    }

    private BasketDto MapBasketToDto(Basket basket)
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