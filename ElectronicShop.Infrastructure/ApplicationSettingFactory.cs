namespace ElectronicShop.Infrastructure
{
    public class ApplicationSettingFactory
    {
        private static IApplicationSetting _iApplicationSetting;

        public static void InitializeApplicationSettingsFactory(IApplicationSetting iApplicationSettings)
        {
            _iApplicationSetting = iApplicationSettings;
        }

        public static IApplicationSetting GetApplicationSettings()
        {
            return _iApplicationSetting;
        }
    }
}