using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Text;

namespace AlbinMicroService.Libraries.Common.QueryManager
{
    public static class SqlQueryCache
    {
        private static readonly Lazy<FrozenDictionary<string, string>> _queries = new(() => LoadQueries(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static FrozenDictionary<string, string> Queries => _queries.Value;

        private static string? _basePath;

        /// <summary>
        /// Initialize the cache with a custom base path (call this from Program.cs)
        /// </summary>
        /// <param name="basePath">The base directory path containing SQL files</param>
        public static void Initialize(string basePath)
        {
            _basePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            // Force initialization
            _ = Queries;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetQuery(string key) => Queries.GetValueOrDefault(key) ?? string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetQuery(string key, out string? query) => Queries.TryGetValue(key, out query);

        private static FrozenDictionary<string, string> LoadQueries()
        {
            string basePath = _basePath ?? Path.Combine(AppContext.BaseDirectory, "Domain", "SqlQueries");

            if (!Directory.Exists(basePath))
            {
                return FrozenDictionary<string, string>.Empty;
            }

            Dictionary<string, string> result = new(StringComparer.OrdinalIgnoreCase);

            // Gets ALL .sql files from the folder (not subdirectories)
            string[] files = Directory.GetFiles(basePath, "*.sql", SearchOption.TopDirectoryOnly);

            Console.WriteLine($"Loading {files.Length} SQL files from: {basePath}");

            foreach (string file in files)
            {
                Console.WriteLine($"Processing: {Path.GetFileName(file)}");
                ProcessFile(file, result);
            }

            Console.WriteLine($"Loaded {result.Count} SQL queries into cache");

            return result.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
        }

        private static void ProcessFile(string filePath, Dictionary<string, string> result)
        {
            // Use full constructor for performance and encoding detection
            using StreamReader reader = new(
                path: filePath,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 8192
            );

            string? currentKey = null;
            StringBuilder sb = new(1024);

            string? line;

            while ((line = reader.ReadLine()) != null)
            {
                ReadOnlySpan<char> trimmedLine = line.AsSpan().TrimStart();

                if (trimmedLine.StartsWith("-- name:".AsSpan(), StringComparison.OrdinalIgnoreCase))
                {
                    // Save the previous query
                    if (currentKey != null && sb.Length > 0)
                    {
                        result[currentKey] = sb.ToString().Trim();
                        sb.Clear();
                    }

                    // Extract the new key
                    int colonIndex = line.IndexOf(':', StringComparison.Ordinal);
                    if (colonIndex != -1 && colonIndex + 1 < line.Length)
                    {
                        currentKey = line[(colonIndex + 1)..].Trim();
                    }
                }
                else if (currentKey != null)
                {
                    sb.AppendLine(line);
                }
            }

            // Capture the last SQL query
            if (currentKey != null && sb.Length > 0)
            {
                result[currentKey] = sb.ToString().Trim();
            }
        }
    }
}
