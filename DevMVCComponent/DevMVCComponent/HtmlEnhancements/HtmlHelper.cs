using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using DevMvcComponent.Extensions;

namespace DevMvcComponent.HtmlEnhancements {
    /// <summary>
    /// </summary>
    public static class HtmlHelper {
        private static string GetStyles(string property, string value) {
            return StringExtension.DependingStringConcat(value, property, ":", value);
        }

        /// <summary>
        ///     Get common styles string value pair.
        /// </summary>
        /// <param name="backgroundColor">Pass only background color-value if exist.  Eg. black</param>
        /// <param name="color">Pass only color-value if exist. Eg. white</param>
        /// <param name="margin">Pass only (margin-value: top right bottom left) if exist. Eg. 2px 2px 2px 2px</param>
        /// <param name="padding">Pass only (padding-value: top right bottom left) if exist.</param>
        /// <param name="borderRadius">Pass only (border-radius-value: value) if exist.</param>
        /// <returns></returns>
        public static string GetCommonStyles(string backgroundColor, string color, string margin, string padding,
            string borderRadius, string fontWeight = null) {
            var stlesList = new List<string>(8);
            stlesList.Add(GetStyles("background-color", backgroundColor));
            stlesList.Add(GetStyles("color", color));
            stlesList.Add(GetStyles("margin", margin));
            stlesList.Add(GetStyles("padding", padding));
            stlesList.Add(GetStyles("border-radius", borderRadius));
            stlesList.Add(GetStyles("font-weight", fontWeight));
            return string.Join(";", stlesList);
        }

        /// <summary>
        ///     Get tag (not self closing) with Html contents
        /// </summary>
        /// <param name="tag">Tag name</param>
        /// <param name="content">Content of html</param>
        /// <param name="styles">Give only style values</param>
        /// <param name="htmlAttributes">Your html attributes</param>
        /// <returns>return Html string.</returns>
        public static string GetTag(string tag, string content, string styles = null, object htmlAttributes = null) {
            return GetTag(false, tag, content, styles, htmlAttributes);
        }

        /// <summary>
        ///     Get self closing tag with html contents.
        /// </summary>
        /// <param name="tag">Tag name</param>
        /// <param name="content">Content of html</param>
        /// <param name="styles">Give only style values</param>
        /// <param name="htmlAttributes">Your html attributes</param>
        /// <returns>return Html string.</returns>
        public static string GetTagSelfClose(string tag, string content, string styles = null, object htmlAttributes = null) {
            return GetTag(true, tag, content, styles, htmlAttributes);
        }

        /// <summary>
        ///     Get a tag with Html contents.
        /// </summary>
        /// <param name="selfClosing">Is it self closing or not.</param>
        /// <param name="tag">Tag name</param>
        /// <param name="content">Content of html</param>
        /// <param name="styles">Give only style values</param>
        /// <param name="htmlAttributes">Your html attributes</param>
        /// <returns>return Html string.</returns>
        public static string GetTag(bool selfClosing, string tag, string content, string styles = null, object htmlAttributes = null) {
            var builder = new TagBuilder(tag);
            builder.InnerHtml = content;
            if (styles != null) {
                builder.MergeAttribute("styles", styles);
            }

            if (styles != null) {
                builder.MergeAttribute("styles", styles);
            }
            if (htmlAttributes != null) {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }
            if (selfClosing) {
                return builder.ToString(TagRenderMode.SelfClosing);
            }
            return builder.ToString();
        }
    }
}