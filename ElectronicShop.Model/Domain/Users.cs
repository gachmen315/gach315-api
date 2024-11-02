using ElectronicShop.Model.ComplexType;
using Microsoft.AspNetCore.Identity;

namespace ElectronicShop.Model.Domain
{
    public class Users : IdentityUser
    {
        public UserType UserType { get; set; }

        public ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
    }
}