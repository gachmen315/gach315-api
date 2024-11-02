using Authen.Model.UserInfoModels;
using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Model.UserInfoModels
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public string InCharge { get; set; } = string.Empty;

        public UserInfoViewModel ConvertToViewModel() => new()
        {
            Email = this.Email,
            FullName = this.FullName,
            Id = this.Id,
            InCharge = this.InCharge,
            PhoneNumber = this.PhoneNumber,
            Status = this.Status,
            UserName = this.UserName,
        };
    }

    public class UserInfos : UserInfo
    {
        public string RoleName { get; set; } = string.Empty;
        public int TotalRows { get; set; }

        public new UserInfoViewModel ConvertToViewModel() => new()
        {
            Email = this.Email,
            FullName = this.FullName,
            Id = this.Id,
            RoleName = this.RoleName,
            InCharge = this.InCharge,
            PhoneNumber = this.PhoneNumber,
            Status = this.Status,
            UserName = this.UserName,
        };
    }

    public class UserInfoViewModel : UserInfo
    {
        public string? RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public IEnumerable<UserOrgViewModel> UserOrgs { get; set; } = Enumerable.Empty<UserOrgViewModel>();
    }

    public class CreateUserReq
    {
        [Required(ErrorMessage = "Tên tài khoản trống!")]
        [RegularExpression(@"^[a-zA-Z0-9._]{4,}$", ErrorMessage = "Tên tài khoản không đúng định dạng! Ít nhất phải có 4 ký tự, chỉ cho phép ký tự thường, ký tự viết hoa, ký tự số, dấu chấm ('.') và dấu gạch dưới ('_')")]
        [MinLength(4, ErrorMessage = "Tên tài khoản không đúng định dạng! Ít nhất phải có 4 ký tự!")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email trống!")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu trống!")]
        [MinLength(6, ErrorMessage = "Mật khẩu không đủ mạnh! Ít nhất phải có 6 ký tự!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,}$", ErrorMessage = "Mật khẩu không đủ mạnh! Ít nhất phải có 6 ký tự, trong đó có  1 ký tự số, 1 ký tự viết hoa và 1 ký tự đặc biệt")]
        public string Password { get; set; } = string.Empty;

        public UserStatus Status { get; set; }
        public string? FullName { get; set; }

        [MinLength(10, ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string? PhoneNumber { get; set; }

        public string? RoleId { get; set; }
        public string? InCharge { get; set; } = "Chưa có";

        public IEnumerable<UserOrgViewModel> OrgCodes { get; set; } = Enumerable.Empty<UserOrgViewModel>();

        public UserInfoModify ConvertToRequestModel(string? User) => new()
        {
            UserName = this.UserName,
            Email = this.Email,
            FullName = this.FullName,
            InCharge = this.InCharge ?? "Chưa có",
            PhoneNumber = this.PhoneNumber,
            Status = this.Status,
            User = string.IsNullOrWhiteSpace(User) ? "System" : User,
        };
    }

    public class UserOrgViewModel
    {
        public string NetworkCode { get; set; } = string.Empty;
        public IEnumerable<string> SpecialtyCodes { get; set; } = Enumerable.Empty<string>();
    }

    public static class AuthenMapper
    {
        public static IEnumerable<UserOrg> ConvertToModel(this IEnumerable<UserOrgViewModel> reqs)
        {
            foreach (var req in reqs)
            {
                foreach (var spect in req.SpecialtyCodes)
                {
                    yield return new UserOrg()
                    {
                        NetworkCode = req.NetworkCode,
                        SepcialtyCode = spect
                    };
                }
            }
        }

        public static IEnumerable<UserOrgViewModel> ConvertToViewModel(this IEnumerable<UserOrg> data)
        {
            return data.GroupBy(d => d.NetworkCode).Select(s => new UserOrgViewModel()
            {
                NetworkCode = s.Key,
                SpecialtyCodes = data.Where(i => i.NetworkCode == s.Key && !string.IsNullOrWhiteSpace(i.SepcialtyCode)).Select(i => i.SepcialtyCode)
            });
        }
    }

    public class UserOrg
    {
        public string NetworkCode { get; set; } = string.Empty;
        public string SepcialtyCode { get; set; } = string.Empty;
    }

    public class UpdateUserReq
    {
        [Required(ErrorMessage = "Tên tài khoản trống!")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email trống!")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng!")]
        public string Email { get; set; } = string.Empty;

        public UserStatus Status { get; set; }
        public string? FullName { get; set; }

        [MinLength(10, ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string? PhoneNumber { get; set; }

        public string? RoleId { get; set; }
        public string? InCharge { get; set; } = "Chưa có";
        public IEnumerable<UserOrgViewModel> OrgCodes { get; set; } = Enumerable.Empty<UserOrgViewModel>();

        public UserInfoModify ConvertToRequestModel(string? User) => new()
        {
            UserName = this.UserName,
            Email = this.Email,
            FullName = this.FullName,
            InCharge = this.InCharge ?? "Chưa có",
            PhoneNumber = this.PhoneNumber,
            Status = this.Status,
            User = string.IsNullOrWhiteSpace(User) ? "System" : User,
        };
    }

    public class UserInfoModify
    {
        public string UserName { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public UserStatus Status { get; set; }
        public string InCharge { get; set; } = string.Empty;
        public string User { get; set; } = "System";
    }

    public class UserInfoSearch : PagingRequest
    {
        public UserStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}