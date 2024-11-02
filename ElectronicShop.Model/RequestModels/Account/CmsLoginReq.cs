using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.RequestModels.Account
{
    public class CmsLoginReq
    {
        [Required(ErrorMessage = "Tên tài khoản trống!")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password trống!")]
        [MinLength(6, ErrorMessage = "Password không đúng định dạng! Ít nhất phải có 6 ký tự!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,200}$", ErrorMessage = "Password không đúng định dạng! Ít nhất phải có 6 ký tự, trong đó có  1 ký tự số, 1 ký tự viết hoa và 1 ký tự đặc biệt")]
        public string Password { get; set; } = string.Empty;
    }
}