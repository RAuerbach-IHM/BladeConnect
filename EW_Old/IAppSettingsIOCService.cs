namespace EW
{
    public interface IAppSettingsIOCService
    {
        void SetAppSettings(AppSettings appSettings);

        AppSettings GetAppSettings();
    }
}
