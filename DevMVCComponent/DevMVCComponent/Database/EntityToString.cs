#region using block

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

#endregion

namespace DevMVCComponent.Database {
    public class EntityToString {
        public static string Get(object Class) {
            var output = "";
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(typeOfPropertise)
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

        public static string GetHtml(object Class) {
            var output = "";
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(typeOfPropertise)
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
        ///     Generating single row of this object.
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        public static string GetHtmlTableRow(object Class, int? count = null) {
            var output = "";
            //generating single row.
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(typeOfPropertise)
                        .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");
                output += "<tr>";
                byte count2 = 0;
                foreach (var prop in propertise) {
                    count2++;
                    var val = prop.GetValue(Class, null);
                    var str = "";
                    if (count != null && count2 == 1) {
                        //generate serial col.
                        str += String.Format("<td style=\"{0}\">{1}</td>", TdCss, count);
                    }

                    if (DataTypeSupport.IsSupport(val)) {
                        str += String.Format("<td style=\"{0}\">{1}</td>", TdCss, val);
                    }
                    output += str;
                }
                output += "</tr>";
            }
            return output;
        }

        private static string GetHtmlTableHeader(object Class) {
            var output = "";
            //generating single row for headers.
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {
                var propertise =
                    Class.GetType()
                        .GetProperties(typeOfPropertise)
                        .Where(p => p.Name != "EntityKey" && p.Name != "EntityState");
                output += "<tr>";
                var count = 0;
                foreach (var prop in propertise) {
                    count++;
                    var val = prop.GetValue(Class, null);
                    var str = "";
                    if (count == 1) {
                        //generate serial number
                        str += String.Format("<th style=\"{0}\">{1}</th>", ThCss, "SL.");
                    }
                    if (DataTypeSupport.IsSupport(val)) {
                        str += String.Format("<th style=\"{0}\">{1}</th>", ThCss, prop.Name);
                    }
                    output += str;
                }
                output += "</tr>";
            }
            return output;
        }

        public static string GetHtmlOfEntities(IEnumerable<object> classes, string tableCaption = "") {
            if (classes == null)
                return "";

            var output = string.Format("<h1 style=\"{0}\">Total Items : {1}</h1><table style=\"{2}\">", TableCaptionCss,
                classes.Count(), TableCss);
            if (tableCaption != "")
                output += string.Format("<caption style=\"{0}\">{1}</caption>", TableCaptionCss, tableCaption);

            var count = 0;
            foreach (var classObject in classes.ToList()) {
                count++;
                if (count == 1) {
                    //generate Table Header.
                    output += GetHtmlTableHeader(classObject);
                }
                output += GetHtmlTableRow(classObject, count);
            }
            output += "</table>";
            return output;
        }

        private static string GetHtmlOfEntitiesEmailGenerate(IEnumerable<object> classes, string email, string sub,
            string tableCaption = "") {
            if (classes == null || !classes.Any())
                return "";
            var output = GetHtmlOfEntities(classes, tableCaption);
            if (Starter.Mailer != null) {
                Starter.Mailer.QuickSend(email, sub, output);
            }

            return output;
        }

        public static void GetHtmlOfEntitiesEmail(IEnumerable<object> classes, string email, string sub,
            string tableCaption = "") {
            if (classes == null || !classes.Any())
                return;
            var thread = new Thread(() => GetHtmlOfEntitiesEmailGenerate(classes, email, sub, tableCaption));
            thread.Start();
        }

        #region Declarations

        private const string FontCss =
            "font-size: 12px ;font-family: 'Segoe UI','Calibri', 'Sans-Serif', 'Lucida Grande' ,'Trebuchet MS','Verdana','Arial';";

        private const string TableCss = "border: solid 1px #E8EEF4; border-collapse: collapse;" + FontCss;

        private const string ThCss =
            "padding: 6px 5px;text-align: left;background-color:#E8EEF4;border: solid 1px #E8EEF4;" + FontCss;

        private const string TdCss = "padding: 5px;border: solid 1px #E8EEF4;" + FontCss;

        private const string TableCaptionCss =
            " background-color: #D0DEC2;font-weight: bold;padding: 5px;text-align: center;" + FontCss;

        #endregion
    }
}