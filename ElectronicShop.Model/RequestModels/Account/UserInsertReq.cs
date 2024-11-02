using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RequestModels.Account
{
    public class UserInsertReq
    {
        public string? Avatar { get; set; }

        [Required(ErrorMessage = "Tên tài khoản trống!")]
        [RegularExpression(@"^[a-zA-Z0-9._]{4,}$", ErrorMessage = "UserName không đúng định dạng! Ít nhất phải có 4 ký tự, chỉ cho phép ký tự thường, ký tự viết hoa, ký tự số, dấu chấm ('.') và dấu gạch dưới ('_')")]
        [MinLength(4, ErrorMessage = "UserName không đúng định dạng! Ít nhất phải có 4 ký tự!")]
        public string UserName { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Password trống!")]
        [MinLength(6, ErrorMessage = "Password không đủ mạnh! Ít nhất phải có 6 ký tự!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,}$", ErrorMessage = "Password không đủ mạnh! Ít nhất phải có 6 ký tự, trong đó có  1 ký tự số, 1 ký tự viết hoa và 1 ký tự đặc biệt")]
        public string? Password { get; set; }

        //[Required(ErrorMessage = "Email trống!")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Họ tên trống!")]
        public string? FullName { get; set; }

        //[Required(ErrorMessage = "Số điện thoại trống!")]
        [RegularExpression("^(0[9|8|7|5|3|])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ!")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string? PhoneNumber { get; set; }

        //[Required(ErrorMessage = "Vai trò trống!")]
        public string? RoleId { get; set; }
    }
}