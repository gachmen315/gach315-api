namespace ElectronicShop.Model.ResponseModels.Account
{
    public class InternalUser
    {
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class InternalUsers : InternalUser
    {
        public int TotalRows { get; set; }
        public string UserType { get; set; } = string.Empty;
        public string? RoleName { get; set; }

        public InternalUsersViewModel ConvertToViewModel() => new()
        {
            Id = Id,
            Avatar = Avatar,
            UserName = UserName,
            UserId = UserId,
            PhoneNumber = PhoneNumber,
            Email = Email,
            FullName = FullName,
            Status = Status,
            UserType = UserType,
            RoleName = RoleName
        };
    }

    public class InternalUsersViewModel
    {
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RoleName { get; set; }
        public string UserType { get; set; } = string.Empty;
    }

    public class UserViewModel : InternalUser
    {
        public string? RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}