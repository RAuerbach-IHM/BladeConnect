namespace EW
{
    public class AppSettingsIOCService : IAppSettingsIOCService
    {
        public AppSettingsIOCService()
        {
        }

        public AppSettings AppSettings;

        public void SetAppSettings(AppSettings appSettings)
        {
            AppSettings = appSettings;
        }

        public AppSettings GetAppSettings()
        {
            return AppSettings;
        }
    }
}
