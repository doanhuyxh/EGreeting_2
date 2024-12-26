using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EGreeting.Services
{
    public static class SlugHelper
    {
        public static string ToSlug(this string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return string.Empty;
            }
            title = title.ToLowerInvariant()
                         .Normalize(NormalizationForm.FormD)
                         .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                         .Aggregate(string.Empty, (current, c) => current + c);
            title = Regex.Replace(title, @"[^a-z0-9\s-]", "")
                         .Trim()
                         .Replace(' ', '-');
            title = Regex.Replace(title, @"-+", "-");
            return title;

        }

    }
}
