namespace AlbinMicroService.Users.Domain
{
    public class AppSettings
    {
        public LoggingSettings Logging { get; init; } = new();
        public string AllowedHosts { get; init; } = string.Empty;
        public SwaggerSettings Swagger { get; init; } = new();
        public ConnectionStringSettings ConnectionStrings { get; init; } = new();
        public JwtSettings Jwt { get; init; } = new();
        public MailSettings Email { get; init; } = new();
    }

    public class LoggingSettings
    {
        public LogLevelSettings LogLevel { get; init; } = new();
    }

    public class LogLevelSettings
    {
        public string Default { get; init; } = string.Empty;
        public string MicrosoftAspNetCore { get; init; } = string.Empty;
    }

    public class SwaggerSettings
    {
        public bool Enabled { get; init; }
        public bool RequireAuthentication { get; init; }
        public string[] AllowedRoles { get; init; } = [];
    }
    public class ConnectionStringSettings
    {
        public string SqlServer { get; init; } = string.Empty;
    }

    public class JwtSettings
    {
        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int TokenExpiryMinutes { get; init; }
    }
    public class MailSettings
    {
        public string EmailPassword { get; init; } = string.Empty;
        public string FromEmail { get; init; } = string.Empty;
        public string SmtpServer { get; init; } = string.Empty;
        public int SmtpPort { get; init; }
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
