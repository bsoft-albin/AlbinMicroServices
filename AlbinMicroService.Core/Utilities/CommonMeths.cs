using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using MailKit.Net.Smtp;
using MimeKit;

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

    public interface IDynamicMeths
    {
        /// <summary>
        /// Hashes a string using Argon2.
        /// </summary>
        public string HashString(string input);
        /// <summary>
        /// Verifies if the input matches the stored Argon2 hash.
        /// </summary>
        public bool VerifyHash(string input, string storedHash);
        Task<bool> SendEmailAsync(EmailTemplate emailTemplate);
    }

    public class DynamicMeths : IDynamicMeths
    {
        #region Props
        private const int MemorySize = 65536; // 64MB of RAM usage
        private const int Iterations = 4; // Number of hashing iterations
        private const int Parallelism = 2; // Number of parallel threads
        #endregion

        #region Meths
        
        public string HashString(string input)
        {
            var config = new Argon2Config
            {
                MemoryCost = MemorySize,
                TimeCost = Iterations,
                Lanes = Parallelism,
                Threads = Parallelism,
                Password = Encoding.UTF8.GetBytes(input)
            };

            using (Argon2 argon2 = new(config))
            {
                using (SecureArray<byte> hashBytes = argon2.Hash())
                {
                    return Convert.ToBase64String(hashBytes.Buffer);
                }
            }
        }
        
        public bool VerifyHash(string input, string storedHash)
        {
            return Argon2.Verify(storedHash, input);
        }

        public async Task<bool> SendEmailAsync(EmailTemplate emailTemplate)
        {
            ArgumentNullException.ThrowIfNull(emailTemplate, nameof(emailTemplate));

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailTemplate.Title, emailTemplate.FromEmail));
                message.To.Add(new MailboxAddress(emailTemplate.Username, emailTemplate.ToEmail));
                message.Subject = emailTemplate.Subject;

                // Create Email Body (HTML)
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = emailTemplate.Body
                };
                message.Body = bodyBuilder.ToMessageBody();

                // Send Email via SMTP
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(emailTemplate.SmtpServer, emailTemplate.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(emailTemplate.FromEmail, emailTemplate.EmailPassword);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
