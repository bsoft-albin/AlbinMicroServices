namespace AlbinMicroService.Users.Domain
{
    public class AppSettings
    {
        public LoggingSettings Logging { get; set; } = new();
        public string AllowedHosts { get; set; } = string.Empty;
        public SwaggerSettings Swagger { get; set; } = new();
        public ConnectionStringSettings ConnectionStrings { get; set; } = new();
        public JwtSettings Jwt { get; set; } = new();
    }

    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; set; } = new();
    }

    public class LogLevelSettings
    {
        public string Default { get; set; } = string.Empty;
        public string MicrosoftAspNetCore { get; set; } = string.Empty;
    }

    public class SwaggerSettings
    {
        public bool Enabled { get; set; }
        public bool RequireAuthentication { get; set; }
        public string[] AllowedRoles { get; set; } = [];
    }
    public class ConnectionStringSettings
    {
        public string SqlServer { get; set; } = string.Empty;
    }

    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpiryMinutes { get; set; }
    }

    // Preventing using a static class modifications (use a static helper if needed)
    public static class WebAppConfigs
    {
        public static AppSettings Settings { get; private set; } = new();

        public static void Initialize(AppSettings settings)
        {
            Settings = settings;
        }
    }

}
