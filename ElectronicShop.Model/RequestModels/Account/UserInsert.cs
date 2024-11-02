namespace ElectronicShop.Model.RequestModels.Account
{
    public class UserInsert
    {
        public string? Avatar { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string Status { get; set; } = string.Empty;
        public string User { get; set; } = "system";
    }
}