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
    }

    public class DynamicProps
    {

    }

}
