using System;
using System.IO;
using System.Text;
using System.Web;

namespace DevMvcComponent.Miscellaneous {
    /// <summary>
    /// Directory extensions
    /// Consist of solutions http://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
    /// </summary>
    public static class DirectoryExtension {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetBaseOrAppDirectory() {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory() {
            return Directory.GetCurrentDirectory();
        }
        /// <summary>
        /// Returns web's root directory absolute location.
        /// </summary>
        /// <returns></returns>
        public static string GetWebAppRootDirectory() {
            if (HttpContext.Current != null) {
                return HttpContext.Current.Server.MapPath(@"~\");
            }
            return "";
        }

        /// <summary>
        /// Get web directory absolute path.
        /// </summary>
        /// <param name="relativePath">Give web directory relative path using telda sign, E.g. ~\AppData</param>
        /// <returns></returns>
        public static string GetWebAppDirectory(string relativePath) {
            return HttpContext.Current.Server.MapPath(relativePath);
        }



        /// <summary>
        /// Get a string display what all methods returns.
        /// </summary>
        /// <returns></returns>
        public static string GetAllDirectoriesList() {
            var sb = new StringBuilder(8);
            sb.AppendLine("GetBaseOrAppDirectory() : ");
            sb.Append(GetBaseOrAppDirectory());
            sb.AppendLine();

            sb.AppendLine("GetCurrentDirectory() : ");
            sb.Append(GetCurrentDirectory());
            sb.AppendLine();

            sb.AppendLine("GetWebAppRootDirectory() : ");
            sb.Append(GetWebAppRootDirectory());
            sb.AppendLine();



            return sb.ToString();
        }
    }
}