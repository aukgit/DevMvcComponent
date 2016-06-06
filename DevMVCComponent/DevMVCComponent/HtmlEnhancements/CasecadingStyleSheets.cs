using System;
using System.Collections.Generic;
using System.Text;

namespace DevMvcComponent.HtmlEnhancements {
    /// <summary>
    /// </summary>
    public static class CasecadingStyleSheets {
        /// <summary>
        ///     Get inline css as string.
        /// </summary>
        /// <param name="cssStyles"></param>
        /// <returns></returns>
        public static string GetInline(Dictionary<string, string> cssStyles) {
            var sb = new StringBuilder(cssStyles.Count * 4 + 5);

            foreach (var style in cssStyles) {
                sb.Append(style.Key);
                sb.Append(":");
                sb.Append(style.Value);
                sb.Append(";");
            }
            var result = sb.ToString();
            sb = null;
            GC.Collect();
            return result;
        }
    }
}