namespace ElectronicShop.Model.RequestModels.Account
{
    public class CheckPermissionReq
    {
        public string Username { get; set; } = string.Empty;
        public string Permission { get; set; } = string.Empty;
    }

    public class RefreshTokenReq
    {
        public string RefreshToken { get; set; }
    }
}