namespace DevMvcComponent.HtmlEnhancements
{
    /// <summary>
    ///     Common css style object.
    /// </summary>
    public class CssProperties
    {
        public CssProperties()
        { }

        /// <summary>
        /// </summary>
        /// <param name="backgroundColor">Pass only background color-value if exist.  Eg. black</param>
        /// <param name="color">Pass only color-value if exist. Eg. white</param>
        /// <param name="margin">Pass only (margin-value: top right bottom left) if exist. Eg. 2px 2px 2px 2px</param>
        /// <param name="padding">Pass only (padding-value: top right bottom left) if exist.</param>
        /// <param name="borderRadius">Pass only (border-radius-value: value) if exist.</param>
        /// <param name="fontSize">font size with value.</param>
        /// <param name="fontWeight"></param>
        /// <param name="textDecoration"></param>
        /// <param name="textAlignment"></param>
        /// <param name="display"></param>
        public CssProperties(
            string backgroundColor = null,
            string color = null,
            string margin = null,
            string padding = null,
            string borderRadius = null,
            string fontSize = null,
            string fontWeight = null,
            string textDecoration = null,
            string textAlignment = null,
            string display = null)
        {
            BackgroundColor = backgroundColor;
            Color           = color;
            Margin          = margin;
            Padding         = padding;
            BorderRadius    = borderRadius;
            FontSize        = fontSize;
            FontWeight      = fontWeight;
            TextDecoration  = textDecoration;
            TextAlignment   = textAlignment;
            Display         = display;
        }

        /// <summary>
        ///     Hex code or RGB both works
        /// </summary>
        public string BackgroundColor { get; set; }

        /// <summary>
        ///     Hex code or RGB both works
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        ///     Set margin : top right bottom left
        /// </summary>
        public string Margin { get; set; }

        /// <summary>
        ///     Set padding : top right bottom left
        /// </summary>
        public string Padding { get; set; }

        /// <summary>
        ///     Set radius : 4px or anything
        /// </summary>
        public string BorderRadius { get; set; }

        /// <summary>
        ///     It is best to have fonts in em size.
        /// </summary>
        public string FontSize { get; set; }

        /// <summary>
        ///     Bold, normal, bolder or 400,500,700
        /// </summary>
        public string FontWeight { get; set; }

        /// <summary>
        ///     None, underline, overline, line-through
        /// </summary>
        public string TextDecoration { get; set; }

        /// <summary>
        ///     left, center, right
        /// </summary>
        public string TextAlignment { get; set; }

        /// <summary>
        ///     none, block, inline, inline-block, table, table-cell...
        /// </summary>
        public string Display { get; set; }
    }
}