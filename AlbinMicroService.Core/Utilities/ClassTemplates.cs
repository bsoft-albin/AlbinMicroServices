namespace AlbinMicroService.Core.Utilities
{
    public class ValidatorTemplate
    {
        public bool IsValidated { get; set; }
        public List<string> Errors { get; set; } = [];
    }

    public class EmailTemplate(int smtpPort, string smtpServer, string fromEmail, string emailPassword, string toEmail)
    {
        public int SmtpPort { get; set; } = smtpPort;
        public string SmtpServer { get; set; } = smtpServer;
        public string FromEmail { get; set; } = fromEmail;
        public string EmailPassword { get; set; } = emailPassword;
        public string ToEmail { get; set; } = toEmail;
        /// <summary>
        /// Gets or sets the Username of the Receiver.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        public string Title { get; set; } = "Email Notification";
        public string Body { get; set; } = "<h1>This is an email notification<h1>";
        public string Subject { get; set; } = string.Empty;
    }
}
