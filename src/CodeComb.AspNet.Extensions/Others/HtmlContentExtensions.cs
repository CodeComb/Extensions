using System.IO;
using Microsoft.Extensions.WebEncoders;

namespace Microsoft.AspNet.Mvc.Rendering
{
    internal class CodeCombHtmlEncoder : IHtmlEncoder
    {
        public string HtmlEncode(string value)
        {
            return value;
        }

        public void HtmlEncode(string value, int startIndex, int charCount, TextWriter output)
        {
            output.Write(value.Substring(startIndex, charCount));
        }

        public void HtmlEncode(char[] value, int startIndex, int charCount, TextWriter output)
        {
            output.Write(value, startIndex, charCount);
        }
    }
    public static class HtmlContentExtensions
    {
        public static string ToHtmlString(this TagBuilder self)
        {
            using (var writer = new StringWriter())
            {
                self.WriteTo(writer, new CodeCombHtmlEncoder());
                return writer.ToString();
            }
        }
    }
}
