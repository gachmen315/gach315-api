using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RoleModels
{
    public class RoleInsertReq
    {
        [Required(ErrorMessage = "Tên vai trò trống!")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public IEnumerable<string> PermissionCodes { get; set; } = Enumerable.Empty<string>();
    }
}