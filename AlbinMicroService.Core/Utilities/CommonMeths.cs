namespace AlbinMicroService.Core.Utilities
{
    public static class StaticMeths
    {
        public static void SetGlobalWebAppMode(bool isDev, bool isStaging, bool isProduction)
        {
            StaticProps.GlobalWebAppRunningMode.IsDev = isDev;
            StaticProps.GlobalWebAppRunningMode.IsStaging = isStaging;
            StaticProps.GlobalWebAppRunningMode.IsProduction = isProduction;
        }
    }

    public class DynamicMeths
    {

    }
}
