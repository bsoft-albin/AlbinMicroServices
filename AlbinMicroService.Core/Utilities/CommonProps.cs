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

        public static class GlobalWebAppSettings
        {
            public static string DefaultUserRole { get; set; } = null!;
            public static bool EnableAuditLogs { get; set; }
            public static string SupportEmail { get; set; } = null!;
            public static int MaxConcurrentUsers { get; set; }
            public static bool EnableMaintenanceMode { get; set; }
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

}
