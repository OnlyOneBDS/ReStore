using Stripe;
namespace ReStore.Svc.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class PaymentsController : BaseApiController
  {
    private readonly PaymentService _paymentService;
    private readonly StoreContext _context;
    private readonly IConfiguration _config;

    public PaymentsController(PaymentService paymentService, StoreContext context, IConfiguration config)
    {
      _context = context;
      _paymentService = paymentService;
      _config = config;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent()
    {
      var basket = await _context.Baskets.RetrieveBasketWithItems(User.Identity.Name).FirstOrDefaultAsync();

      if (basket == null)
      {
        return NotFound();
      }

      var intent = await _paymentService.CreateOrUpdatePaymentIntent(basket);

      if (intent == null)
      {
        return BadRequest(new ProblemDetails { Title = "Problem creatign payment intent" });
      }

      basket.PaymentIntentId = basket.PaymentIntentId ?? intent.Id;
      basket.ClientSecret = basket.ClientSecret ?? intent.ClientSecret;

      _context.Update(basket);

      var result = await _context.SaveChangesAsync() > 0;

      if (!result)
      {
        return BadRequest(new ProblemDetails { Title = "Problem updating basket with intent" });
      }

      return basket.MapBasketToDto();
    }

    [HttpPost("webhook")]
    public async Task<ActionResult> StripeWebhook()
    {
      var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
      var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _config["StripeSettings:WhSecret"]);
      var charge = (Charge)stripeEvent.Data.Object;
      var order = await _context.Orders.FirstOrDefaultAsync(o => o.PaymentIntentId == charge.PaymentIntentId);

      if (charge.Status == "Succeeded")
      {
        order.OrderStatus = OrderStatus.PaymentReceived;
      }

      await _context.SaveChangesAsync();

      return new EmptyResult();
    }
  }
}