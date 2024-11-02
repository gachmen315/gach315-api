using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Model.ResponseModels.Account
{
    public class ProfileViewModel : InternalUser
    {
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
        public IEnumerable<string> GrantedAuthorities { get; set; } = Enumerable.Empty<string>();
    }
}
