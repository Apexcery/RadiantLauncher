namespace ValorantLauncher.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceAll(this string str, string[] oldChars, string newChar)
        {
            foreach (var s in oldChars)
            {
                str = str.Replace(s, newChar);
            }

            return str;
        }
    }
}
