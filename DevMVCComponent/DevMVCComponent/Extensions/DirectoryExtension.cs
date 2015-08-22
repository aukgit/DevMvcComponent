

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace DevMvcComponent.Extensions {
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
            return HttpContext.Current.Server.MapPath(@"~\");
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
        /// Get current assembly's code base method.
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCodeBaseDirectory() {
            return Config.Assembly.CodeBase;
        }

        /// <summary>
        /// Returns the executing directory.
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingDirectory() {
            return Path.GetFullPath("");
        }

        /// <summary>
        /// Get a string display what all methods returns.
        /// </summary>
        /// <returns></returns>
        public static string GetAllDirectoriesList() {
            var sb = new StringBuilder(8);
            sb.AppendLine("GetBaseOrAppDirectory() : ");
            sb.Append(GetBaseOrAppDirectory());

            sb.AppendLine("GetCurrentDirectory() : ");
            sb.Append(GetCurrentDirectory());

            sb.AppendLine("GetWebAppRootDirectory() : ");
            sb.Append(GetWebAppRootDirectory());

            sb.AppendLine("GetAssemblyCodeBaseDirectory() : ");
            sb.Append(GetAssemblyCodeBaseDirectory());

            sb.AppendLine("GetExecutingDirectory() : ");
            sb.Append(GetExecutingDirectory());
            return sb.ToString();
        }
    }
}