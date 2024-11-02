using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RoleModels
{
    public class RoleAssignReq
    {
        [Required(ErrorMessage = "Mã Vai trò trống!")]
        public string RoleId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username trống!")]
        public string Username { get; set; } = string.Empty;
    }
}