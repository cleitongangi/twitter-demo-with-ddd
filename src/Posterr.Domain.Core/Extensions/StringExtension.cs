namespace Posterr.Domain.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToUpperFirstLetter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            if (text.Length == 1)
                return text.ToUpper();
            else
                return char.ToUpper(text[0]) + text[1..];
        }
    }
}
