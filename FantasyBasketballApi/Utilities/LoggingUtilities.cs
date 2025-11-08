namespace FantasyBasketballApi.Utilities;

/// <summary>
/// Utility methods for logging operations
/// </summary>
public static class LoggingUtilities
{
    /// <summary>
    /// Sanitizes a string for safe logging by removing control characters and limiting length
    /// This prevents log forging attacks where malicious input could inject false log entries
    /// </summary>
    /// <param name="input">The input string to sanitize</param>
    /// <returns>A sanitized string safe for logging</returns>
    public static string SanitizeForLogging(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;
        
        // Remove newlines, carriage returns, and other control characters that could be used for log injection
        // Also filter to ASCII characters only to prevent encoding issues
        var sanitized = new string(input.Where(c => !char.IsControl(c) && c < 127).ToArray());
        
        // Limit length to prevent log flooding
        return sanitized.Length > 200 ? sanitized.Substring(0, 200) : sanitized;
    }
}
