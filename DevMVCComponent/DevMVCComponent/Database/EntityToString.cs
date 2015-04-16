using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DevMVCComponent.Database {
    public class EntityToString {

        #region Declarations
        const string font_css = "font-size: 12px ;font-family: 'Segoe UI','Calibri', 'Sans-Serif', 'Lucida Grande' ,'Trebuchet MS','Verdana','Arial';";
        const string table_css = "border: solid 1px #E8EEF4; border-collapse: collapse;" + font_css;
        const string th_css = "padding: 6px 5px;text-align: left;background-color:#E8EEF4;border: solid 1px #E8EEF4;" + font_css;
        const string td_css = "padding: 5px;border: solid 1px #E8EEF4;" + font_css;
        const string table_caption_css = " background-color: #D0DEC2;font-weight: bold;padding: 5px;text-align: center;" + font_css;
        #endregion

        public static string Get(object Class) {
            string output = "";
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {

                var propertise = Class.GetType().GetProperties(typeOfPropertise).Where(p => p.Name != "EntityKey" && p.Name != "EntityState");

                foreach (var prop in propertise) {
                    object val = prop.GetValue(Class, null);
                    string str = "";
                    if (DataTypeSupport.isSupport(val)) {
                        str = String.Format("\n{0} : {1}", prop.Name.ToString(), val);
                        //Console.WriteLine(str);
                    }
                    output += str;
                }
                //output += "\n";
            }
            return output;
        }

        public static string GetHTML(object Class) {
            string output = "";
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {

                var propertise = Class.GetType().GetProperties(typeOfPropertise).Where(p => p.Name != "EntityKey" && p.Name != "EntityState");

                foreach (var prop in propertise) {
                    object val = prop.GetValue(Class, null);
                    string str = "";
                    if (DataTypeSupport.isSupport(val)) {
                        str = String.Format("<br/>{0} : {1}", prop.Name.ToString(), val);
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
        /// Generating single row of this object.
        /// </summary>
        /// <param name="Class"></param>
        /// <returns></returns>
        public static string GetHTMLTableRow(object Class, int? count = null) {
            string output = "";
            //generating single row.
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {

                var propertise = Class.GetType().GetProperties(typeOfPropertise).Where(p => p.Name != "EntityKey" && p.Name != "EntityState");
                output += "<tr>";
                byte count2 = 0;
                foreach (var prop in propertise) {
                    count2++;
                    object val = prop.GetValue(Class, null);
                    string str = "";
                    if (count != null && count2 == 1) {
                        //generate serial col.
                        str += String.Format("<td style=\"{0}\">{1}</td>", td_css, count);
                    }

                    if (DataTypeSupport.isSupport(val)) {
                        str += String.Format("<td style=\"{0}\">{1}</td>", td_css, val);
                    }
                    output += str;
                }
                output += "</tr>";
            }
            return output;
        }

        private static string GetHTMLTableHeader(object Class) {
            string output = "";
            //generating single row for headers.
            var typeOfPropertise = BindingFlags.Public | BindingFlags.Instance;
            if (Class != null) {
                var propertise = Class.GetType().GetProperties(typeOfPropertise).Where(p => p.Name != "EntityKey" && p.Name != "EntityState");
                output += "<tr>";
                int count = 0;
                foreach (var prop in propertise) {
                    count++;
                    object val = prop.GetValue(Class, null);
                    string str = "";
                    if (count == 1) {
                        //generate serial number
                        str += String.Format("<th style=\"{0}\">{1}</th>", th_css, "SL.");
                    }
                    if (DataTypeSupport.isSupport(val)) {
                        str += String.Format("<th style=\"{0}\">{1}</th>", th_css, prop.Name.ToString());
                    }
                    output += str;
                }
                output += "</tr>";
            }
            return output;
        }

        public static string GetHTMLOfEntities(IEnumerable<object> Classes, string tableCaption = "") {
            if (Classes == null)
                return "";

            string output = string.Format("<h1 style=\"{0}\">Total Items : {1}</h1><table style=\"{2}\">", table_caption_css, Classes.Count(), table_css);
            if (tableCaption != "")
                output += string.Format("<caption style=\"{0}\">{1}</caption>", table_caption_css, tableCaption);

            int count = 0;
            foreach (var classObject in Classes.ToList()) {
                count++;
                if (count == 1) {
                    //generate Table Header.
                    output += GetHTMLTableHeader(classObject);
                }
                output += GetHTMLTableRow(classObject, count);
            }
            output += "</table>";
            return output;
        }

        private static string GetHTMLOfEntitiesEmailGenerate(IEnumerable<object> Classes, string email, string sub, string tableCaption = "") {
            if (Classes == null || !Classes.Any())
                return "";
            string output = GetHTMLOfEntities(Classes, tableCaption);
            if (Starter.Mailer != null) {
                Starter.Mailer.QuickSend(email, sub, output);
            }

            return output;
        }
        public static void GetHTMLOfEntitiesEmail(IEnumerable<object> Classes, string email, string sub, string tableCaption = "") {
            if (Classes == null || !Classes.Any())
                return;
            Thread thread = new Thread(() => GetHTMLOfEntitiesEmailGenerate(Classes, email, sub, tableCaption));
            thread.Start();
        }

    }
}