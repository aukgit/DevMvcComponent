#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using DevMvcComponent.DataTypeFormat;
using DevMvcComponent.HtmlEnhancements;

#endregion

namespace DevMvcComponent.EntityConversion
{
    /// <summary>
    ///     Convert any database entity to html string for email sending.
    /// </summary>
    public static class EntityToString
    {
        private const BindingFlags TypeOfPropertise = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        ///     Convert any object to string based on property and values string list.
        ///     Mostly convert any entity object to property and values string list, escape EntityKey and EntityState type fields.
        ///     DevMvcComponent.EntityConversion.EntityToString extension method.
        /// </summary>
        /// <param name="anyObject">Any entity object , can be null.</param>
        /// <param name="propertyFormat">{0} : property, {1} value of the property.</param>
        /// <returns>
        ///     Returns property and value string combination, returns empty string if class is null or doesn't have any valid
        ///     properties to display.
        /// </returns>
        public static string AsString(this object anyObject, string propertyFormat = "\n{0} : {1}") => Get(anyObject, propertyFormat);

        /// <summary>
        ///     Convert any object to string based on property and values string list.
        ///     Mostly convert any entity object to property and values string list, escape EntityKey and EntityState type fields.
        ///     DevMvcComponent.EntityConversion.EntityToString extension method.
        /// </summary>
        /// <param name="anyObject">Any entity object , can be null.</param>
        /// <param name="propertyFormat">{0} : property, {1} value of the property.</param>
        /// <returns>
        ///     Returns property and value string combination, returns empty string if class is null or doesn't have any valid
        ///     properties to display.
        /// </returns>
        public static string Get(object anyObject, string propertyFormat = "\n{0} : {1}")
        {
            if (anyObject != null)
            {
                var propertise =
                    anyObject.GetType()
                             .GetProperties(TypeOfPropertise)
                             .Where(p => p.Name != "EntityKey" && p.Name != "EntityState")
                             .ToList();

                var sb = new StringBuilder(propertise.Count + 2);

                foreach (var prop in propertise)
                {
                    var val = prop.GetValue(anyObject, null);

                    if (TypeChecker.IsPrimitiveOrGuid(val))
                    {
                        var str = string.Format(propertyFormat, prop.Name, val);

                        //Console.WriteLine(str);
                        sb.AppendLine(str);
                    }
                }

                var output = sb.ToString();
                sb = null;
                GC.Collect();

                return output;
            }

            return "";
        }

        /// <summary>
        ///     Get simple Html string of a single class object with only new lines
        /// </summary>
        /// <param name="Class">Any entity object , can be null.</param>
        /// <param name="additionalStylesWithTable"></param>
        /// <param name="additionalStylesWithRow"></param>
        /// <param name="additionalStylesWithCell"></param>
        /// <returns>
        ///     Returns property and value string combination, returns empty string if class is null or doesn't have any valid
        ///     properties to display.
        /// </returns>
        public static string AsHtmlTable(
            object Class,
            string additionalStylesWithTable = "",
            string additionalStylesWithRow = "",
            string additionalStylesWithCell = "")
        {
            if (Class != null)
            {
                var properties =
                    Class.GetType()
                         .GetProperties(TypeOfPropertise)
                         .Where(p => p.Name != "EntityKey" && p.Name != "EntityState")
                         .ToList();

                var sb = new StringBuilder(properties.Count * 3 + 20);

                var styles = CssStyle.GetCommonStyles(
                    "#74DA74",
                    "black",
                    null,
                    "8px 20px 16px",
                    "4px");

                var styles2 = CssStyle.GetCommonStyles(
                    null,
                    null,
                    null,
                    null,
                    null,
                    "bolder");

                sb.Append("<table ");
                sb.Append("style='" + styles + additionalStylesWithTable + "'>");
                sb.Append("<thead>");

                //sb.Append(HtmlHelperExtension.GetTag("caption", "Entity Title : " + anyObject.ToString()));
                sb.Append("<tr ");
                sb.Append("style='" + styles2 + additionalStylesWithRow + "'>");
                sb.Append(HtmlHelperExtension.GetTag("td", "", additionalStylesWithCell));
                sb.Append(HtmlHelperExtension.GetTag("td", "", additionalStylesWithCell));
                sb.Append(HtmlHelperExtension.GetTag("td", "", additionalStylesWithCell));

                //sb.Append(HtmlHelperExtension.GetTag("td", "Properties"));
                //sb.Append(HtmlHelperExtension.GetTag("td", ""));
                //sb.Append(HtmlHelperExtension.GetTag("td", "Values"));
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                foreach (var prop in properties)
                {
                    var val = prop.GetValue(Class, null);
                    sb.Append("<tr ");
                    sb.Append("style='" + additionalStylesWithRow + "'>");

                    if (TypeChecker.IsPrimitiveOrGuid(val))
                    {
                        sb.Append(HtmlHelperExtension.GetTag("td", prop.Name, additionalStylesWithCell));
                        sb.Append(":");
                        sb.Append(HtmlHelperExtension.GetTag("td", val.ToString(), additionalStylesWithCell));
                    }

                    sb.Append("</tr>");
                }

                sb.Append("</tbody>");
                sb.Append("</table>");
                var output = sb.ToString();
                sb = null;
                GC.Collect();

                return output;
            }

            return "";
        }

        /// <summary>
        ///     Get simple Html string of a single class object with only new lines
        /// </summary>
        /// <param name="Class">Any entity object , can be null.</param>
        /// <param name="additionalStylesWithTable">add inline styles with table.</param>
        /// <param name="additionalStylesWithRow">add inline styles with tr element or row.</param>
        /// <param name="additionalStylesWithCell">add inline styles with td element or cell.</param>
        /// <returns>
        ///     Returns property and value string combination, returns empty string if class is null or doesn't have any valid
        ///     properties to display.
        /// </returns>
        public static string GetHtmlOfSingleClassAsTable(
            object Class,
            string additionalStylesWithTable = "",
            string additionalStylesWithRow = "",
            string additionalStylesWithCell = null)
        {
            if (Class != null)
            {
                var properties =
                    Class.GetType()
                         .GetProperties(TypeOfPropertise)
                         .Where(p => p.Name != "EntityKey" && p.Name != "EntityState")
                         .ToList();

                var sb = new StringBuilder(properties.Count * 3 + 20);

                var styles = CssStyle.GetCommonStyles(
                    "#74DA74",
                    "black",
                    null,
                    "8px 20px 16px",
                    "4px");

                var styles2 = CssStyle.GetCommonStyles(
                    null,
                    null,
                    null,
                    null,
                    null,
                    "bolder");

                sb.Append("<table ");
                sb.Append("style='" + styles + additionalStylesWithTable + "'>");
                sb.Append("<thead>");

                //sb.Append(HtmlHelperExtension.GetTag("caption", "Entity Title : " + anyObject.ToString()));
                sb.Append("<tr ");
                sb.Append("style='" + styles2 + additionalStylesWithRow + "'>");
                sb.Append(HtmlHelperExtension.GetTag("td", ""));
                sb.Append(HtmlHelperExtension.GetTag("td", ""));
                sb.Append(HtmlHelperExtension.GetTag("td", ""));

                //sb.Append(HtmlHelperExtension.GetTag("td", "Properties"));
                //sb.Append(HtmlHelperExtension.GetTag("td", ""));
                //sb.Append(HtmlHelperExtension.GetTag("td", "Values"));
                sb.Append("</tr>");
                sb.Append("</thead>");
                sb.Append("<tbody>");

                foreach (var prop in properties)
                {
                    var val = prop.GetValue(Class, null);
                    sb.AppendLine("<tr>");

                    if (TypeChecker.IsPrimitiveOrGuid(val))
                    {
                        sb.Append(HtmlHelperExtension.GetTag("td", prop.Name, additionalStylesWithCell));
                        sb.Append(HtmlHelperExtension.GetTag("td", ":", additionalStylesWithCell));
                        sb.Append(HtmlHelperExtension.GetTag("td", val.ToString(), additionalStylesWithCell));
                    }

                    sb.Append("</tr>");
                }

                sb.Append("</tbody>");
                sb.Append("</table>");
                var output = sb.ToString();
                sb = null;
                GC.Collect();

                return output;
            }

            return "";
        }

        /// <summary>
        ///     Get simple Html string of a single class object with only new lines
        /// </summary>
        /// <param name="Class">Any entity object , can be null.</param>
        /// <returns>
        ///     Returns property and value string combination, returns empty string if class is null or doesn't have any valid
        ///     properties to display.
        /// </returns>
        public static string GetHtmlOfSingleClass(object Class)
        {
            if (Class != null)
            {
                var propertise =
                    Class.GetType()
                         .GetProperties(TypeOfPropertise)
                         .Where(p => p.Name != "EntityKey" && p.Name != "EntityState")
                         .ToList();

                var sb = new StringBuilder(propertise.Count + 2);

                foreach (var prop in propertise)
                {
                    var val = prop.GetValue(Class, null);

                    if (TypeChecker.IsPrimitiveOrGuid(val))
                    {
                        var str = string.Format("<br/>{0} : {1}", prop.Name, val);

                        //Console.WriteLine(str);
                        sb.AppendLine(str);
                    }
                }

                var output = sb.ToString();
                sb = null;
                GC.Collect();

                return output;
            }

            return "";
        }

        // public static string GetHTML(object anyObject , byte StructDisplayFormattype) {
        //    if(StructDisplayFormattype == StructDisplayFormatType.Regular){
        //        return GetHTML(anyObject);
        //    } else {
        //        return GetHTMLTableRow(anyObject);
        //    }
        //}

        /// <summary>
        ///     Generating single row from a list of entities.
        /// </summary>
        /// <param name="Class">Single object</param>
        public static void GetHtmlTableRow(
            object Class,
            ref StringBuilder sb,
            int? count = null)
        {
            //generating single row.
            if (Class != null)
            {
                var propertise =
                    Class.GetType()
                         .GetProperties(TypeOfPropertise)
                         .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");

                sb.AppendLine("<tr>");
                byte count2 = 0;

                foreach (var prop in propertise)
                {
                    count2++;
                    var val = prop.GetValue(Class, null);

                    if (count != null &&
                        count2 == 1)
                    {
                        //generate serial col.
                        sb.AppendLine(string.Format("<td style=\"{0}\">{1}</td>", TdCss, count));
                    }

                    if (TypeChecker.IsPrimitiveOrGuid(val))
                    {
                        sb.AppendLine(string.Format("<td style=\"{0}\">{1}</td>", TdCss, val));
                    }
                }

                sb.AppendLine("</tr>");
            }
        }

        /// <summary>
        ///     Generates the html table header rows for single class properties.
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="sb"></param>
        private static void GetHtmlTableHeader(object Class, ref StringBuilder sb)
        {
            //generating single row for headers.
            if (Class != null)
            {
                var propertise =
                    Class.GetType()
                         .GetProperties(TypeOfPropertise)
                         .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");

                sb.AppendLine("<tr>");
                var count = 0;

                foreach (var prop in propertise)
                {
                    count++;
                    var val = prop.GetValue(Class, null);

                    if (count == 1)
                    {
                        //generate serial number
                        sb.AppendLine(string.Format("<th style=\"{0}\">{1}</th>", ThCss, "SL."));
                    }

                    if (TypeChecker.IsPrimitiveOrGuid(val))
                    {
                        sb.AppendLine(string.Format("<th style=\"{0}\">{1}</th>", ThCss, prop.Name));
                    }
                }

                sb.AppendLine("</tr>");
            }
        }

        /// <summary>
        ///     Get html table string of any database entity list.
        /// </summary>
        /// <param name="list">List of items</param>
        /// <param name="tableCaption">Table caption for this entity.</param>
        /// <returns>Returns html table string of any database entity list.</returns>
        public static string GetHtmlTableOfEntities(IList<object> list, string tableCaption = "")
        {
            if (list == null)
            {
                return "";
            }

            var sb = new StringBuilder(list.Count + 200);

            sb.Append(
                string.Format(
                    "<h1 style=\"{0}\">Total Items : {1}</h1><table style=\"{2}\">",
                    TableCaptionCss,
                    list.Count,
                    TableCss));

            if (tableCaption != "")
            {
                sb.Append(string.Format("<caption style=\"{0}\">{1}</caption>", TableCaptionCss, tableCaption));
            }

            var count = 0;

            foreach (var item in list)
            {
                count++; // 1 for first time it is one.

                if (count == 1)
                {
                    //only add generate Table Header for first time.
                    GetHtmlTableHeader(item, ref sb);
                }

                GetHtmlTableRow(item, ref sb, count);
            }

            sb.Append("</table>");
            var output = sb.ToString();
            sb = null;
            GC.Collect();

            return output;
        }

        private static string GetHtmlOfEntitiesEmailGenerate(
            IList<object> listObjects,
            string email,
            string sub,
            string tableCaption = "")
        {
            if (listObjects == null ||
                !listObjects.Any())
            {
                return "";
            }

            var output = GetHtmlTableOfEntities(listObjects, tableCaption);

            if (Mvc.Mailer != null)
            {
                //async
                Mvc.Mailer.QuickSend(email, sub, output);
            }

            return output;
        }

        /// <summary>
        ///     Async: Convert database entities list to html string and then
        ///     send to to an email.
        /// </summary>
        /// <param name="list">List of items</param>
        /// <param name="email">Email address to send the email.</param>
        /// <param name="sub">Subject of the email.</param>
        /// <param name="tableCaption">Caption of the table for this entity.</param>
        public static void GetHtmlOfEntitiesEmail(
            IList<object> list,
            string email,
            string sub,
            string tableCaption = "")
        {
            if (list == null ||
                !list.Any())
            {
                return;
            }

            var thread = new Thread(
                () => GetHtmlOfEntitiesEmailGenerate(
                    list,
                    email,
                    sub,
                    tableCaption));

            thread.Start();
        }

        #region Declarations

        /// <summary>
        /// </summary>
        public const string FontCss =
            "font-size: 12px ;font-family: 'Segoe UI','Calibri', 'Sans-Serif', 'Lucida Grande' ,'Trebuchet MS','Verdana','Arial';";

        /// <summary>
        /// </summary>
        public const string TableCss = "border: solid 1px #E8EEF4; border-collapse: collapse;" + FontCss;

        /// <summary>
        /// </summary>
        public const string ThCss =
            "padding: 6px 5px;text-align: left;background-color:#E8EEF4;border: solid 1px #E8EEF4;" + FontCss;

        /// <summary>
        /// </summary>
        public const string TdCss = "padding: 5px;border: solid 1px #E8EEF4;" + FontCss;

        /// <summary>
        /// </summary>
        public const string TableCaptionCss =
            " background-color: #D0DEC2;font-weight: bold;padding: 5px;text-align: center;" + FontCss;

        #endregion
    }
}