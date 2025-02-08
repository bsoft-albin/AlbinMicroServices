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

        public static class CustomHttpStatusCodes
        {
            public static short ServerShutdown { get; set; } = 800;
        }

        public static class CustomHttpStatusMessages
        {
            public static string ServerShutdown { get; set; } = "Temporarily the Server Shutdown.";
            public static string UnXpectedError { get; set; } = "An unexpected error occurred.";
            public static string UnKnownError { get; set; } = "UNKNOWN_ERROR.";
        }
    }

    public class DynamicProps
    {

    }

}
