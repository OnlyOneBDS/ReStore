using Microsoft.AspNetCore.Identity;

namespace ReStore.Svc.Entities
{
  public class User : IdentityUser<int>
  {
    public UserAddress Address { get; set; }
  }
}