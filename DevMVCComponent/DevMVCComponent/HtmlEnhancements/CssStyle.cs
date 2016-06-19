using System;
using System.Collections.Generic;
using System.Text;
using DevMvcComponent.Extensions;
using DevMvcComponent.Extensions.String;

namespace DevMvcComponent.HtmlEnhancements {
    /// <summary>
    ///     Cascading style sheets functionalities.
    /// </summary>
    public static class CssStyle {
        /// <summary>
        ///     Get inline css as string.
        /// </summary>
        /// <param name="cssStyles"></param>
        /// <returns></returns>
        public static string GetInline(Dictionary<string, string> cssStyles) {
            var sb = new StringBuilder(cssStyles.Count * 4 + 5);

            foreach (var style in cssStyles) {
                if (style.Value != null) {
                    sb.Append(style.Key);
                    sb.Append(":");
                    sb.Append(style.Value);
                    sb.Append(";");
                }
            }
            var result = sb.ToString();
            sb = null;
            GC.Collect();
            return result;
        }

        /// <summary>
        ///     Get common styles string value pair.
        /// </summary>
        /// <param name="backgroundColor">Pass only background color-value if exist.  Eg. black</param>
        /// <param name="color">Pass only color-value if exist. Eg. white</param>
        /// <param name="margin">Pass only (margin-value: top right bottom left) if exist. Eg. 2px 2px 2px 2px</param>
        /// <param name="padding">Pass only (padding-value: top right bottom left) if exist.</param>
        /// <param name="borderRadius">Pass only (border-radius-value: value) if exist.</param>
        /// <param name="fontWeight"></param>
        /// <returns>return inline css style string.</returns>
        public static string GetCommonStyles(
            string backgroundColor,
            string color,
            string margin = null,
            string padding = null,
            string borderRadius = null,
            string fontWeight = null) {
            var stylesList = new List<string>(8) {
                GetStyles("background-color", backgroundColor),
                GetStyles("color", color),
                GetStyles("margin", margin),
                GetStyles("padding", padding),
                GetStyles("border-radius", borderRadius),
                GetStyles("font-weight", fontWeight)
            };
            return string.Join(";", stylesList);
        }

        /// <summary>
        ///     Get css style string.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStyles(string property, string value) {
            return value.DependingStringConcat(property, ":", value);
        }

        /// <summary>
        ///     Convert any css propertise to inline css style.
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static string GetInlineStyles(this CssProperties rules) {
            var stylesList = new List<string>(10) {
                GetStyles("background-color", rules.BackgroundColor),
                GetStyles("color", rules.Color),
                GetStyles("margin", rules.Margin),
                GetStyles("padding", rules.Padding),
                GetStyles("border-radius", rules.BorderRadius),
                GetStyles("font-weight", rules.FontWeight),
                GetStyles("font-size", rules.FontSize),
                GetStyles("text-decoration", rules.TextDecoration),
                GetStyles("text-align", rules.TextAlignment),
                GetStyles("display", rules.Display)
            };
            return string.Join(";", stylesList);
        }
    }
}