using Microsoft.Security.Application;

namespace WebApplication.Infrastructure.Security
{
    public static class EncodingExtensions
    {
        public static string HtmlEncode(this string input)
        {
            return Encoder.HtmlEncode(input);
        }

        public static string JavaScriptEncode(this string input)
        {
            return Encoder.JavaScriptEncode(input);
        }

        public static string GetSafeHtml(this string input)
        {
            return Sanitizer.GetSafeHtml(input);
        }

        public static string GetSafeHtmlFragment(this string input)
        {
            return Sanitizer.GetSafeHtmlFragment(input);
        }
    }
}
