using Microsoft.AspNetCore.Identity;

namespace ElectronicShop.Model.Domain
{
    public class Role : IdentityRole
    {
        public Role(string name, string? description) : base(name)
        {
            Description = description;
        }

        public string? Description { get; set; }
        public ICollection<IdentityRoleClaim<string>> Claims { get; set; } = new List<IdentityRoleClaim<string>>();
    }
}