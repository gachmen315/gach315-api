namespace ElectronicShop.Infrastructure
{
    public interface IApplicationSetting
    {
        string ConnectionString { get; }
        AppSetting AppSetting { get; }
    }
}