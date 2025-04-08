namespace AlbinMicroService.Core.Utilities
{
    public class ValidatorTemplate
    {
        /// <summary>
        /// Gets or sets the IsValidated
        /// </summary>
        public bool IsValidated { get; set; }
        /// <summary>
        /// Gets or sets the List of Errors
        /// </summary>
        public List<string> Errors { get; set; } = [];
    }

    public class WebAppBuilderConfigTemplate
    {
        public bool IsHavingSSL { get; set; }
        public bool IsSwaggerEnabled { get; set; }
        public bool IsRunningInContainer { get; set; }
        public int HttpsPort { get; set; }
        public string ApiVersion { get; set; } = null!;
        public string ApiTitle { get; set; } = null!;
        public int HttpPort { get; set; }
        public List<int> Ports { get; set; } = [];
    }

    public class EmailTemplate(int smtpPort, string smtpServer, string fromEmail, string emailPassword, string toEmail)
    {
        /// <summary>
        /// Gets or sets the SmtpPort
        /// </summary>
        public int SmtpPort { get; set; } = smtpPort;
        /// <summary>
        /// Gets or sets the SmtpServer
        /// </summary>
        public string SmtpServer { get; set; } = smtpServer;
        /// <summary>
        /// Gets or sets the FromEmail
        /// </summary>
        public string FromEmail { get; set; } = fromEmail;
        /// <summary>
        /// Gets or sets the EmailPassword
        /// </summary>
        public string EmailPassword { get; set; } = emailPassword;
        /// <summary>
        /// Gets or sets the ToEmail
        /// </summary>
        public string ToEmail { get; set; } = toEmail;
        /// <summary>
        /// Gets or sets the Username of the Receiver
        /// </summary>
        public string Username { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; } = "Email Notification";
        /// <summary>
        /// Gets or sets the Body
        /// </summary>
        public string Body { get; set; } = "<h1>This is an email notification<h1>";
        /// <summary>
        /// Gets or sets the Subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;
    }
}
