﻿namespace ElectronicShop.Model.ResponseModels.Account
{
    public class Token
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}