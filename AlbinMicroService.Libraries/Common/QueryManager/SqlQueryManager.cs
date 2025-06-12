using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AlbinMicroService.Libraries.Common.QueryManager
{
    public static class SqlQueryManager
    {
        private static readonly ConcurrentDictionary<string, Dictionary<string, string>> _queryCache = new();
        private static readonly Regex _queryRegex = new(@"--\s*@(\w+)\s*\n(.*?)(?=--\s*@\w+|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets SQL query by file name and query name
        /// </summary>
        /// <param name="fileName">SQL file name without extension (e.g., "UserQueries")</param>
        /// <param name="queryName">Query name defined in SQL file (e.g., "GetUserById")</param>
        /// <param name="callingAssembly">Assembly containing SQL files (auto-detected if null)</param>
        /// <returns>SQL query string</returns>
        public static string GetQuery(string fileName, string queryName, Assembly callingAssembly = null)
        {
            callingAssembly ??= Assembly.GetCallingAssembly();
            var cacheKey = $"{callingAssembly.GetName().Name}:{fileName}";

            var queries = _queryCache.GetOrAdd(cacheKey, _ => LoadQueriesFromFile(fileName, callingAssembly));

            if (queries.TryGetValue(queryName, out string query))
            {
                return query;
            }

            throw new ArgumentException($"Query '{queryName}' not found in file '{fileName}'. Available queries: {string.Join(", ", queries.Keys)}");
        }

        /// <summary>
        /// Preloads all SQL files from assembly for maximum performance
        /// </summary>
        public static void PreloadQueries(Assembly assembly, string sqlFolderName = "SqlQueries")
        {
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(name => name.Contains($".{sqlFolderName}.") && name.EndsWith(".sql"));

            Parallel.ForEach(resourceNames, resourceName =>
            {
                var fileName = ExtractFileName(resourceName, sqlFolderName);
                var cacheKey = $"{assembly.GetName().Name}:{fileName}";

                if (!_queryCache.ContainsKey(cacheKey))
                {
                    _queryCache.TryAdd(cacheKey, LoadQueriesFromFile(fileName, assembly, sqlFolderName));
                }
            });
        }

        /// <summary>
        /// Gets all available files and their queries
        /// </summary>
        public static Dictionary<string, string[]> GetAvailableQueries(Assembly assembly, string sqlFolderName = "SqlQueries")
        {
            var result = new Dictionary<string, string[]>();
            var resourceNames = assembly.GetManifestResourceNames()
                .Where(name => name.Contains($".{sqlFolderName}.") && name.EndsWith(".sql"));

            foreach (var resourceName in resourceNames)
            {
                var fileName = ExtractFileName(resourceName, sqlFolderName);
                var cacheKey = $"{assembly.GetName().Name}:{fileName}";
                var queries = _queryCache.GetOrAdd(cacheKey, _ => LoadQueriesFromFile(fileName, assembly, sqlFolderName));
                result[fileName] = [.. queries.Keys];
            }

            return result;
        }

        private static Dictionary<string, string> LoadQueriesFromFile(string fileName, Assembly assembly, string sqlFolderName = "SqlQueries")
        {
            var assemblyName = assembly.GetName().Name;
            var resourceName = $"{assemblyName}.{sqlFolderName}.{fileName}.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                var availableResources = assembly.GetManifestResourceNames()
                    .Where(name => name.EndsWith(".sql"))
                    .ToArray();

                throw new FileNotFoundException(
                    $"SQL file not found: {resourceName}\n" +
                    $"Available SQL files: {string.Join(", ", availableResources)}");
            }

            using var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            return ParseQueries(content, fileName);
        }

        private static Dictionary<string, string> ParseQueries(string content, string fileName)
        {
            var queries = new Dictionary<string, string>();
            var matches = _queryRegex.Matches(content);

            if (matches.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No queries found in {fileName}.sql. " +
                    "Queries should be defined with '-- @QueryName' comments.");
            }

            foreach (Match match in matches)
            {
                var queryName = match.Groups[1].Value.Trim();
                var queryText = match.Groups[2].Value.Trim();

                if (string.IsNullOrWhiteSpace(queryText))
                {
                    throw new InvalidOperationException($"Query '{queryName}' in {fileName}.sql is empty");
                }

                queries[queryName] = queryText;
            }

            return queries;
        }

        private static string ExtractFileName(string resourceName, string sqlFolderName)
        {
            var parts = resourceName.Split('.');
            var sqlIndex = Array.FindIndex(parts, p => p == sqlFolderName);

            if (sqlIndex >= 0 && sqlIndex < parts.Length - 1)
            {
                // Support nested folders: AssemblyName.SqlQueries.Users.UserQueries.sql -> Users.UserQueries
                return string.Join(".", parts.Skip(sqlIndex + 1).Take(parts.Length - sqlIndex - 2));
            }

            return parts[^2]; // Fallback
        }
    }
}
