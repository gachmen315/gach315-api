using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RequestModels.Account
{
    public class UserUpdateReq : UserInsertReq
    {
        [Required(ErrorMessage = "Mã tài khoản trống!")]
        public string UserId { get; set; } = string.Empty;

        [MinLength(6, ErrorMessage = "Password không đủ mạnh! Ít nhất phải có 6 ký tự!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,200}$", ErrorMessage = "Password không đủ mạnh! Ít nhất phải có 6 ký tự, trong đó có  1 ký tự số, 1 ký tự viết hoa và 1 ký tự đặc biệt")]
        public new string? Password { get; set; }

        [Required(ErrorMessage = "Trạng thái trống!")]
        public string Status { get; set; } = string.Empty;

        public new string? RoleId { get; set; }
    }

    public class UserUpdateStatusReq
    {
        [Required(ErrorMessage = "Tên tài khoản trống!")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trạng thái trống!")]
        public string Status { get; set; } = string.Empty;
    }

    public class UserUpdate
    {
        public string? Avatar { get; set; }

        public string UserId { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string Status { get; set; } = string.Empty;
        public string User { get; set; } = "system";
    }
}