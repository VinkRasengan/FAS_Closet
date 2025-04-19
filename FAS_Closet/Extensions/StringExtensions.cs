using System;

namespace FASCloset.Extensions
{
    /// <summary>
    /// Provides extension methods for string manipulation and validation operations
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks if a string contains another string using a specified comparison type
        /// </summary>
        /// <param name="source">The source string to search within</param>
        /// <param name="value">The substring to search for</param>
        /// <param name="comparisonType">The type of string comparison to use (case-sensitive, culture-specific, etc.)</param>
        /// <returns>True if the substring was found; otherwise, false. Returns false if source is null.</returns>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
        
        /// <summary>
        /// Safely trims a string with null checking to prevent NullReferenceException
        /// </summary>
        /// <param name="source">The string to trim</param>
        /// <returns>The trimmed string, or empty string if source is null</returns>
        public static string SafeTrim(this string source)
        {
            return source?.Trim() ?? string.Empty;
        }
        
        /// <summary>
        /// Truncates a string to a specified maximum length and adds ellipsis if needed
        /// </summary>
        /// <param name="source">The string to truncate</param>
        /// <param name="maxLength">The maximum allowed length of the string</param>
        /// <returns>
        /// The original string if its length is less than or equal to maxLength;
        /// otherwise, a truncated string with ellipsis appended. Returns empty string if source is null.
        /// </returns>
        public static string Truncate(this string source, int maxLength)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            return source.Length <= maxLength ? source : source.Substring(0, maxLength) + "...";
        }
    }
}
