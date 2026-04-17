using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FresherMisa2026.Application.Extensions
{
    public static class SQLExtension
    {
        private static readonly ConcurrentDictionary<string, string> _queryCache = new();
        private static string _currentFilePath;

        /// <summary>
        /// Initialize query manager với file path
        /// </summary>
        public static void Initialize(string filePath = "Queries/Query.json")
        {
            _currentFilePath = filePath;

            LoadQueries();
        }

        /// <summary>
        /// Get raw SQL query by key
        /// </summary>
        public static string GetQuery(string queryKey)
        {
            try
            {
                if (_queryCache.TryGetValue(queryKey, out var query))
                {
                    return query;
                }

                throw new KeyNotFoundException($"Query '{queryKey}' not found in {_currentFilePath}");
            }
            finally
            {
            }
        }

        /// <summary>
        /// Check if query exists
        /// </summary>
        public static bool QueryExists(this string queryKey)
        {
            try
            {
                return _queryCache.ContainsKey(queryKey);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Get all available query keys
        /// </summary>
        public static IEnumerable<string> GetAllQueryKeys()
        {
            try
            {
                return _queryCache.Keys.ToList();
            }
            finally
            {
            }
        }

        /// <summary>
        /// Reload queries from file
        /// </summary>
        public static void ReloadQueries()
        {
            LoadQueries();
        }

        private static void LoadQueries()
        {
            if (string.IsNullOrEmpty(_currentFilePath))
            {
                _currentFilePath = "Queries/Query.json";
            }

            if (!File.Exists(_currentFilePath))
            {
                throw new FileNotFoundException($"Query file not found: {_currentFilePath}");
            }

            try
            {
                var json = File.ReadAllText(_currentFilePath);
                var queries = JsonSerializer.Deserialize<Dictionary<string, string>>(json, new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                });

                _queryCache.Clear();

                foreach (var (key, value) in queries)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        // Normalize query (trim và format)
                        var normalizedQuery = NormalizeQuery(value);
                        _queryCache[key] = normalizedQuery;
                    }
                }


                Console.WriteLine($"✅ Loaded {_queryCache.Count} queries from {_currentFilePath}");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Invalid JSON in query file: {ex.Message}", ex);
            }
            finally
            {
            }
        }

        private static string NormalizeQuery(string query)
        {
            // Remove extra whitespaces và normalize line endings
            return string.Join(" ", query.Split(new[] { '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                        .Trim();
        }
    }
}
