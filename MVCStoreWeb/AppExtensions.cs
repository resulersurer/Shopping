using System.Text.RegularExpressions;

namespace MVCStoreWeb
{
    public enum Roles
    {
        Administrators, ProductManagers, OrderManagers, Members
    }

    public static class AppExtentions
    {
        public static string ToSafeUrlString(this string text)
        {
            return Regex.Replace(string.Concat(text.Where(p => char.IsLetterOrDigit(p) || char.IsWhiteSpace(p))), @"\s+", "-").ToLower();
        }
    }
}
