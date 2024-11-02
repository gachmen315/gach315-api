namespace ElectronicShop.Model.ResponseModels.Account
{
    public class RefreshTokenRes
    {
        public string Message { get; set; }
        public Token? Result { get; set; }
    }
}