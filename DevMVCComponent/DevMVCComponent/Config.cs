#region using block

using System.Reflection;

#endregion

namespace DevMVCComponent {
    /// <summary>
    ///     Must setup this class.
    /// </summary>
    public static class Config {
        /// <summary>
        ///     System admin email
        /// </summary>
        public static string AdminEmail = null;

        /// <summary>
        ///     Developer email
        /// </summary>
        public static string DeveloperEmail = null;

        /// <summary>
        ///     Sets Assembly = Assembly.GetExecutingAssembly();
        /// </summary>
        public static Assembly Assembly = null;

        /// <summary>
        ///     Running application name
        /// </summary>
        public static string ApplicationName = null;

        /// <summary>
        ///     Notify Developer on Error if true.
        /// </summary>
        public static bool IsNotifyDeveloper = true;

        /// <summary>
        ///     Attach application information from the AssemblyInfo given the Assembly = Assembly.GetExecutingAssembly().
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationNameHtml() {
            string str = "",
                divStart = "<div",
                slashClose = ">",
                divClose = "</div>";
            //styleStart = "style='",
            //colorRed = "color:red",
            //styleJoiner = ";",
            //quoteClose = "'";

            if (Assembly != null) {
                str += divStart + " style='background:black;color:white;' " + slashClose;

                str += divStart + slashClose;
                str += "Application Name : ";
                str += Assembly.FullName;
                str += divClose;

                str += divStart + slashClose;
                str += "Application Location : ";
                str += Assembly.Location;
                str += divClose;

                str += divStart + slashClose;
                str += "Application Version : ";
                str += Assembly.GetName().Version;
                str += divClose;

                str += divClose;
                return str;
            }

            return "";
        }
    }
}