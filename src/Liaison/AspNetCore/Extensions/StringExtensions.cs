namespace Liaison.AspNetCore.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(
            this string value )
        {
            return string.IsNullOrWhiteSpace( value );
        }

        public static bool HasValue(
            this string value )
        {
            return !string.IsNullOrWhiteSpace( value );
        }

        public static string RemoveBraces(
            this string value )
        {
            // because we're not using TrimStart or TrimEnd
            // there maybe some interesting edge cases
            return value.Trim( '{' ).Trim( '}' );
        }
    }
}