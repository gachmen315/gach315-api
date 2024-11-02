namespace ElectronicShop.Common.Model
{
    public class EmailConfig
    {
        public int? Id { get; set; } = null;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string BrandCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}