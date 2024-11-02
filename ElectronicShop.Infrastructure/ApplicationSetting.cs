using Microsoft.Extensions.Configuration;

namespace ElectronicShop.Infrastructure
{
    public class ApplicationSetting : IApplicationSetting
    {
        private readonly IConfigurationRoot _configuration;

        public ApplicationSetting(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString => _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        public AppSetting AppSetting
        {
            get
            {
                var appSetting = _configuration.GetSection("AppSetting");
                var microServiceName = appSetting["MicroServiceName"];
                var gatewayDomain = appSetting["GatewayDomain"];
                if (gatewayDomain.Last() == '/')
                {
                    gatewayDomain.Remove(gatewayDomain.Length - 1, 1);
                }
                bool.TryParse(appSetting["EnableLogInfo"], out bool enableLogInfo);
                return new AppSetting()
                {
                    GetwayDomain = $"{gatewayDomain}/",
                    MicroServiceName = microServiceName,
                    FrontEndDomain = appSetting["FrontEndDomain"] ?? "https://thaco-cms.toponseek.com/",
                    BaseApi = $"{gatewayDomain}/{microServiceName}",
                    WebsiteUrl = appSetting["WebsiteUrl"],
                    Environment = appSetting["Environment"] ?? "local",
                    ImgUrl = (appSetting["ImgUrl"] ?? "{GatewayDomain}/ref-data/").Replace("{GatewayDomain}", gatewayDomain),
                    AuthenService = (appSetting["AuthenService"] ?? "{GatewayDomain}/authen/").Replace("{GatewayDomain}", gatewayDomain),
                    ApiKey = appSetting["ApiKey"] ?? "",
                    EnableLogInfo = enableLogInfo,
                };
            }
        }
    }
}