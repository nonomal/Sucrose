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
    }
}