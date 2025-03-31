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

        public static T ConvertType<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static int GetPropertyCount(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            // Get the type of the object and count the properties
            return obj.GetType().GetProperties().Length;
        }

        public static string GetNullOrEmptyOrWhiteSpaceErrorText(string value = "")
        {
            return value + " cannot be null or empty or whitespace.";
        }
    }

    public interface IDynamicMeths
    {
        /// <summary>
        /// Hashes a string using Argon2.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Hashed string of the input.</returns>
        public string HashString(string input);
        /// <summary>
        /// Verifies if the input matches the stored Argon2 hash.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="storedHash"></param>
        /// <returns>True or False if both string matches.</returns>
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
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input), StaticMeths.GetNullOrEmptyOrWhiteSpaceErrorText(input));
            }

            Argon2Config config = new()
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
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input), StaticMeths.GetNullOrEmptyOrWhiteSpaceErrorText(nameof(input)));
            }
            if (string.IsNullOrWhiteSpace(storedHash))
            {
                throw new ArgumentNullException(nameof(storedHash), StaticMeths.GetNullOrEmptyOrWhiteSpaceErrorText(nameof(storedHash)));
            }

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
