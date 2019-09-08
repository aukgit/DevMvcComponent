using System.Web.Mvc;
using System.Web.Routing;

namespace DevMvcComponent.HtmlEnhancements
{
    /// <summary>
    ///     Html helper class
    /// </summary>
    public static class HtmlHelperExtension
    {
        /// <summary>
        ///     Get tag (not self closing) with Html contents
        /// </summary>
        /// <param name="tag">Tag name</param>
        /// <param name="content">Content of html</param>
        /// <param name="styles">Give only style values</param>
        /// <param name="htmlAttributes">Your html attributes</param>
        /// <returns>return Html string.</returns>
        public static string GetTag(
            string tag,
            string content,
            string styles = null,
            object htmlAttributes = null) =>
            GetTag(
                false,
                tag,
                content,
                styles,
                htmlAttributes);

        /// <summary>
        ///     Get self closing tag with html contents.
        /// </summary>
        /// <param name="tag">Tag name</param>
        /// <param name="content">Content of html</param>
        /// <param name="styles">Give only style values</param>
        /// <param name="htmlAttributes">Your html attributes</param>
        /// <returns>return Html string.</returns>
        public static string GetTagSelfClose(
            string tag,
            string content,
            string styles = null,
            object htmlAttributes = null) =>
            GetTag(
                true,
                tag,
                content,
                styles,
                htmlAttributes);

        /// <summary>
        ///     Get a tag with Html contents.
        /// </summary>
        /// <param name="selfClosing">Is it self closing or not.</param>
        /// <param name="tag">Tag name</param>
        /// <param name="content">Content of html</param>
        /// <param name="styles">Give only style values</param>
        /// <param name="htmlAttributes">Your html attributes</param>
        /// <returns>return Html string.</returns>
        public static string GetTag(
            bool selfClosing,
            string tag,
            string content,
            string styles = null,
            object htmlAttributes = null)
        {
            var builder = new TagBuilder(tag);
            builder.InnerHtml = content;

            if (styles != null)
            {
                builder.MergeAttribute("styles", styles);
            }

            if (styles != null)
            {
                builder.MergeAttribute("styles", styles);
            }

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            if (selfClosing)
            {
                return builder.ToString(TagRenderMode.SelfClosing);
            }

            return builder.ToString();
        }
    }
}