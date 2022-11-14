using System.Text;
using System.Globalization;
namespace SmartCookbook.Domain.Extension;

public static class StringExtension
{
    public static bool IgnoreCaseSensitiveNonSpace(this string origin, string search)
    {
        var index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(origin, search, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

        return index >= 0;
    }

    public static string IgnoreNonSpace(this string text)
    {
        return new string(text.Normalize(NormalizationForm.FormD)
                              .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark).ToArray());
    }
}