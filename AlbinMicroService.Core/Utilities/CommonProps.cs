using System.Xml.Serialization;

namespace AlbinMicroService.Core.Utilities
{
    public static class StaticProps
    {
        public static class GlobalWebAppRunningMode
        {
            public static bool IsDev { get; set; }
            public static bool IsStaging { get; set; }
            public static bool IsProduction { get; set; }
        }

        public static DateTime AppDateTimeNow => DateTime.Now;
        public static DateTime AppDateUtcTimeNow => DateTime.UtcNow;

        public static GlobalWebAppSettings GlobalSettings { get; private set; } = new();

        public static void SetGlobalWebAppSettings()
        {
            string CurrentDirectory = AppContext.BaseDirectory; // Gets the output directory at runtime
            string fullPath = Path.Combine(CurrentDirectory, "GlobalSettings.xml");

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException("Global Settings XML file not found", fullPath);
            }

            XmlSerializer serializer = new(typeof(GlobalWebAppSettings));
            using FileStream stream = new(fullPath, FileMode.Open);
            GlobalSettings = (GlobalWebAppSettings)serializer.Deserialize(stream);
        }
    }

    public class DynamicProps
    {
        #region Props

        #region WebRootPath
        public string IdentityWebRoot { get; set; } = string.Empty;
        public string UsersWebRoot { get; set; } = string.Empty;
        public string MasterWebRoot { get; set; } = string.Empty;
        #endregion

        #region Domain URL
        public string IdentityDomain { get; set; } = string.Empty;
        public string UsersDomain { get; set; } = string.Empty;
        public string MasterDomain { get; set; } = string.Empty;
        #endregion

        #endregion
    }

    [XmlRoot("GlobalSettings")]
    public class GlobalWebAppSettings
    {
        public string DefaultUserRole { get; init; } = null!;
        public bool EnableAuditLogs { get; init; }
        public string SupportEmail { get; init; } = null!;
        public int MaxConcurrentUsers { get; init; }
        public bool EnableMaintenanceMode { get; init; }
    }
}
