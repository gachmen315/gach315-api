namespace ElectronicShop.Infrastructure
{
    public class AppSetting
    {
        public static AppSetting Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                _instance = ApplicationSettingFactory.GetApplicationSettings().AppSetting;
                return _instance;
            }
        }

        private static AppSetting? _instance;

        public string WebsiteUrl { get; set; } = string.Empty;
        public string BaseApi { get; set; } = string.Empty;
        public bool EnableResdisCache { get; set; }
        public int HoursCacheDefault { get; set; }
        public string Environment { get; set; } = string.Empty;
        public string ImgUrl { get; set; } = string.Empty;
        public string AuthenService { get; set; } = string.Empty;
        public string MicroServiceName { get; set; } = string.Empty;
        public string FrontEndDomain { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string GetwayDomain { get; set; } = string.Empty;
        public bool EnableLogInfo { get; set; } = true;
        public bool IsEnableElasticsearch { get; set; }
    }
}