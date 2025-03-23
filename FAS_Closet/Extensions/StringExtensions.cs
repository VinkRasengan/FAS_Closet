using System;

namespace FASCloset.Extensions
{
    /// <summary>
    /// Extension methods for string operations
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if a string contains another string with specified comparison
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="value">The value to search for</param>
        /// <param name="comparisonType">The string comparison type</param>
        /// <returns>True if the string contains the value</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
        
        /// <summary>
        /// Safely trims a string with null checking
        /// </summary>
        /// <param name="source">The source string</param>
        /// <returns>The trimmed string or empty string if null</returns>
        public static string SafeTrim(this string source)
        {
            return source?.Trim() ?? string.Empty;
        }
        
        /// <summary>
        /// Truncates a string to the specified length
        /// </summary>
        /// <param name="source">The source string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <returns>Truncated string with ellipsis if needed</returns>
        public static string Truncate(this string source, int maxLength)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            return source.Length <= maxLength ? source : source.Substring(0, maxLength) + "...";
        }
    }
}
