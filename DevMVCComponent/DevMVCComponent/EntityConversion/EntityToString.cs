#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using DevMVCComponent.Database;

#endregion

namespace DevMVCComponent.EntityConversion {
    /// <summary>
    /// Convert any database entity to html string for email sending.
    /// </summary>
    public class EntityToString {
        const BindingFlags TypeOfPropertise = BindingFlags.Public | BindingFlags.Instance;

        /// <summary>
        /// Get simple html string of a single class object
        /// </summary>
        /// <param name="Class">Any entity object , can be null.</param>
        /// <returns></returns>
        public static string Get(object Class) {
            var output = "";
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(TypeOfPropertise)
                        .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");

                foreach (var prop in propertise) {
                    var val = prop.GetValue(Class, null);
                    var str = "";
                    if (DataTypeSupport.IsSupport(val)) {
                        str = String.Format("\n{0} : {1}", prop.Name, val);
                        //Console.WriteLine(str);
                    }
                    output += str;
                }
                //output += "\n";
            }
            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        public static string GetHtmlOfSingleClass(object Class) {
            var output = "";
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(TypeOfPropertise)
                        .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");

                foreach (var prop in propertise) {
                    var val = prop.GetValue(Class, null);
                    var str = "";
                    if (DataTypeSupport.IsSupport(val)) {
                        str = String.Format("<br/>{0} : {1}", prop.Name, val);
                        //Console.WriteLine(str);
                    }
                    output += str;
                }
                //output += "\n";
            }
            return output;
        }

        // public static string GetHTML(object Class , byte StructDisplayFormattype) {
        //    if(StructDisplayFormattype == StructDisplayFormatType.Regular){
        //        return GetHTML(Class);
        //    } else {
        //        return GetHTMLTableRow(Class);
        //    }
        //}

        /// <summary>
        ///     Generating single row from a list of entities.
        /// </summary>
        /// <param name="Class">Single object</param>
        public static void GetHtmlTableRow(object Class, ref StringBuilder sb, int? count = null) {

            //generating single row.
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(TypeOfPropertise)
                        .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");
                sb.AppendLine("<tr>");
                byte count2 = 0;
                foreach (var prop in propertise) {
                    count2++;
                    var val = prop.GetValue(Class, null);
                    if (count != null && count2 == 1) {
                        //generate serial col.
                        sb.AppendLine(String.Format("<td style=\"{0}\">{1}</td>", TdCss, count));
                    }

                    if (DataTypeSupport.IsSupport(val)) {
                        sb.AppendLine(String.Format("<td style=\"{0}\">{1}</td>", TdCss, val));
                    }
                }
                sb.AppendLine("</tr>");
            }
        }
        /// <summary>
        /// Generates the html table header rows for single class properties.
        /// </summary>
        /// <param name="Class"></param>
        /// <param name="sb"></param>
        private static void GetHtmlTableHeader(object Class, ref StringBuilder sb) {
            //generating single row for headers.
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(TypeOfPropertise)
                        .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");
                sb.AppendLine("<tr>");
                var count = 0;
                foreach (var prop in propertise) {
                    count++;
                    var val = prop.GetValue(Class, null);
                    if (count == 1) {
                        //generate serial number
                        sb.AppendLine(String.Format("<th style=\"{0}\">{1}</th>", ThCss, "SL."));
                    }
                    if (DataTypeSupport.IsSupport(val)) {
                        sb.AppendLine(String.Format("<th style=\"{0}\">{1}</th>", ThCss, prop.Name));
                    }
                }
                sb.AppendLine("</tr>");
            }
        }

        /// <summary>
        /// Get html table string of any database entity list.
        /// </summary>
        /// <param name="classes">List of items</param>
        /// <param name="tableCaption">Table caption for this entity.</param>
        /// <returns>Returns html table string of any database entity list.</returns>
        public static string GetHtmlOfEntities(IEnumerable<object> classes, string tableCaption = "") {
            if (classes == null)
                return "";
            var sb = new StringBuilder(classes.Count() + 200);
            sb.Append(string.Format("<h1 style=\"{0}\">Total Items : {1}</h1><table style=\"{2}\">", TableCaptionCss,
                classes.Count(), TableCss));
            if (tableCaption != "") {
                sb.Append(string.Format("<caption style=\"{0}\">{1}</caption>", TableCaptionCss, tableCaption));
            }
            var count = 0;
            foreach (var classObject in classes.ToList()) {
                count++; // 1 for first time it is one.
                if (count == 1) {
                    //only add generate Table Header for first time.
                    GetHtmlTableHeader(classObject, ref sb);
                }
                GetHtmlTableRow(classObject, ref sb, count);
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        private static string GetHtmlOfEntitiesEmailGenerate(IEnumerable<object> classes, string email, string sub,
            string tableCaption = "") {
            if (classes == null || !classes.Any())
                return "";
            var output = GetHtmlOfEntities(classes, tableCaption);
            if (Starter.Mailer != null) {
                //async
                Starter.Mailer.QuickSend(email, sub, output);
            }

            return output;
        }

        /// <summary>
        /// Async: Convert database entities list to html string and then 
        /// send to to an email. 
        /// </summary>
        /// <param name="classes">List of items</param>
        /// <param name="email">Email address to send the email.</param>
        /// <param name="sub">Subject of the email.</param>
        /// <param name="tableCaption">Caption of the table for this entity.</param>
        public static void GetHtmlOfEntitiesEmail(
            IEnumerable<object> classes,
            string email,
            string sub,
            string tableCaption = "") {
            if (classes == null || !classes.Any())
                return;
            var thread = new Thread(() => GetHtmlOfEntitiesEmailGenerate(classes, email, sub, tableCaption));
            thread.Start();
        }

        #region Declarations

        /// <summary>
        /// 
        /// </summary>
        public const string FontCss =
            "font-size: 12px ;font-family: 'Segoe UI','Calibri', 'Sans-Serif', 'Lucida Grande' ,'Trebuchet MS','Verdana','Arial';";

        /// <summary>
        /// 
        /// </summary>
        public const string TableCss = "border: solid 1px #E8EEF4; border-collapse: collapse;" + FontCss;

        /// <summary>
        /// 
        /// </summary>
        public const string ThCss =
            "padding: 6px 5px;text-align: left;background-color:#E8EEF4;border: solid 1px #E8EEF4;" + FontCss;

        /// <summary>
        /// 
        /// </summary>
        public const string TdCss = "padding: 5px;border: solid 1px #E8EEF4;" + FontCss;

        /// <summary>
        /// 
        /// </summary>
        public const string TableCaptionCss =
            " background-color: #D0DEC2;font-weight: bold;padding: 5px;text-align: center;" + FontCss;

        #endregion
    }
}