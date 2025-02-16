using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;

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
        public string HashString(string input);
        public bool VerifyHash(string input, string storedHash);
    }

    public class DynamicMeths : IDynamicMeths
    {
        #region Props
        private const int MemorySize = 65536; // 64MB of RAM usage
        private const int Iterations = 4; // Number of hashing iterations
        private const int Parallelism = 2; // Number of parallel threads
        #endregion

        #region Meths
        /// <summary>
        /// Hashes a string using Argon2.
        /// </summary>
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
        /// <summary>
        /// Verifies if the input matches the stored Argon2 hash.
        /// </summary>
        public bool VerifyHash(string input, string storedHash)
        {
            return Argon2.Verify(storedHash, input);
        }

        #endregion
    }
}
