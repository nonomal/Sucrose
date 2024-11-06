using System.Text.RegularExpressions;

namespace Sucrose.Shared.Space.Helper
{
    internal static class Regexer
    {
        public static string RemoveNumbers(string Value)
        {
            return Regex.Replace(Value, "[^0-9]", "");
        }

        public static string RemoveLowerCase(string Value)
        {
            return Regex.Replace(Value, "[^a-z]", "");
        }

        public static string RemoveUpperCase(string Value)
        {
            return Regex.Replace(Value, "[^A-Z]", "");
        }

        public static string RemoveExtraSpaces(string Value)
        {
            return Regex.Replace(Value, @"\s+", " ");
        }

        public static string RemovePunctuation(string Value)
        {
            return Regex.Replace(Value, @"\p{P}", "");
        }

        public static string RemoveAlphaNumeric(string Value)
        {
            return Regex.Replace(Value, "[^a-zA-Z0-9]", "");
        }

        public static Match Match(string Value, string Pattern)
        {
            return Regex.Match(Value, Pattern);
        }

        public static bool IsMatch(string Value, string Pattern)
        {
            return Regex.IsMatch(Value, Pattern);
        }

        public static Match Match(string Value, string Pattern, RegexOptions Options)
        {
            return Regex.Match(Value, Pattern, Options);
        }

        public static bool IsMatch(string Value, string Pattern, RegexOptions Options)
        {
            return Regex.IsMatch(Value, Pattern, Options);
        }

        public static string Replace(string Value, string Pattern, string Replacement)
        {
            return Regex.Replace(Value, Pattern, Replacement);
        }

        public static string Replace(string Value, string Pattern, string Replacement, RegexOptions Options)
        {
            return Regex.Replace(Value, Pattern, Replacement, Options);
        }
    }
}