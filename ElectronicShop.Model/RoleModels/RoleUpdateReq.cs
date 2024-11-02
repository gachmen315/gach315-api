using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RoleModels
{
    public class RoleUpdateReq : RoleInsertReq
    {
        [Required(ErrorMessage = "Mã Vai trò trống!")]
        public string Id { get; set; } = string.Empty;
    }
}