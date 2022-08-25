using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReStore.Svc.Data;
using ReStore.Svc.DTOs;
using ReStore.Svc.Entities;
using ReStore.Svc.Extensions;

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
      var basket = await RetrieveBasket(GetBuyerId());

      return basket == null ? NotFound() : basket.MapBasketToDto();
    }

    [HttpPost]
    public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
    {
      // Get basket
      var basket = await RetrieveBasket(GetBuyerId());

      if (basket == null)
      {
        // Create a new basket
        basket = CreateBasket();
      }

      // Get product
      var product = await _context.Products.FindAsync(productId);

      if (product == null)
      {
        return BadRequest(new ProblemDetails { Title = "Product Not Found" });
      }

      // Add item and quantity
      basket.AddItem(product, quantity);

      // Save changes
      var result = await _context.SaveChangesAsync() > 0;

      return result ? CreatedAtRoute("GetBasket", basket.MapBasketToDto()) : BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
    {
      // Get basket
      var basket = await RetrieveBasket(GetBuyerId());

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
      var buyerId = User.Identity?.Name;

      if (string.IsNullOrEmpty(buyerId))
      {
        buyerId = Guid.NewGuid().ToString();

        var cookieOptions = new CookieOptions
        {
          IsEssential = true,
          Expires = DateTime.Now.AddDays(30)
        };

        Response.Cookies.Append("buyerId", buyerId, cookieOptions);
      }

      var basket = new Basket { BuyerId = buyerId };

      _context.Add(basket);

      return basket;
    }

    private async Task<Basket> RetrieveBasket(string buyerId)
    {
      if (string.IsNullOrEmpty(buyerId))
      {
        Response.Cookies.Delete("buyerId");
        return null;
      }

      return await _context.Baskets
        .Include(i => i.Items)
        .ThenInclude(p => p.Product)
        .FirstOrDefaultAsync(x => x.BuyerId == Request.Cookies["buyerId"]);
    }

    private string GetBuyerId() => User.Identity?.Name ?? Request.Cookies["buyerId"];
  }
}