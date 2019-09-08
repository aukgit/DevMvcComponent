using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace DevMvcComponent.Miscellaneous
{
    /// <summary>
    ///     Directory extensions
    ///     Consist of solutions http://stackoverflow.com/questions/6041332/best-way-to-get-application-folder-path
    /// </summary>
    public static class DirectoryExtension
    {
        /// <summary>
        ///     AppDomain.CurrentDomain.BaseDirectory
        /// </summary>
        /// <returns>returns AppDomain.CurrentDomain.BaseDirectory</returns>
        public static string GetBaseOrAppDirectory() => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     HttpRuntime.AppDomainAppPath
        /// </summary>
        /// <returns>returns HttpRuntime.AppDomainAppPath</returns>
        public static string GetAppDomainAppPath() => HttpRuntime.AppDomainAppPath;

        /// <summary>
        ///     new Uri(Config.Assembly.CodeBase).AbsolutePath
        ///     Problem with this that it returns for space in Windows 10 it adds %20%
        ///     So if path has "Hello World" return "Hello%20%World";
        /// </summary>
        /// <returns>return new Uri(Config.Assembly.CodeBase).AbsolutePath</returns>
        public static string GetAbsolutePathFromUri() => new Uri(Config.Assembly.CodeBase).AbsolutePath;

        /// <summary>
        ///     Assembly.GetExecutingAssembly().CodeBase
        ///     Returns permanent assembly running directory.
        /// </summary>
        /// <returns>returns Assembly.GetExecutingAssembly().CodeBase</returns>
        public static string GetAssemblyPath() => Assembly.GetExecutingAssembly().CodeBase;

        /// <summary>
        ///     returns System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath
        /// </summary>
        /// <returns>returns System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath</returns>
        public static string GetApplicationPhysicalPath() => HostingEnvironment.ApplicationPhysicalPath;

        /// <summary>
        ///     Directory.GetCurrentDirectory
        /// </summary>
        /// <returns>returns Directory.GetCurrentDirectory</returns>
        public static string CurrentDirectoryUsingDirectory() => Directory.GetCurrentDirectory();

        /// <summary>
        ///     Returns web's root directory absolute location.
        ///     HttpContext.Current.Server.MapPath(@"~\")
        /// </summary>
        /// <returns>returns HttpContext.Current.Server.MapPath(@"~\")</returns>
        public static string GetWebAppRootDirectory()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(@"~\");
            }

            return "";
        }

        /// <summary>
        ///     Get web directory absolute path.
        ///     HttpContext.Current.Server.MapPath(@"~\")
        /// </summary>
        /// <param name="relativePath">Give web directory relative path using telda sign, E.g. ~\AppData</param>
        /// <returns></returns>
        public static string GetWebAppDirectory(string relativePath) => HttpContext.Current.Server.MapPath(relativePath);

        /// <summary>
        ///     Get a string display what all methods returns.
        /// </summary>
        /// <returns>Get a string display what all methods returns.</returns>
        public static string GetAllDirectoriesList()
        {
            var sb = new StringBuilder(12);
            sb.AppendLine("GetBaseOrAppDirectory() : ");
            sb.Append(GetBaseOrAppDirectory());
            sb.AppendLine();

            sb.AppendLine("GetCurrentDirectory() : ");
            sb.Append(CurrentDirectoryUsingDirectory());
            sb.AppendLine();

            sb.AppendLine("GetAbsolutePathFromUri() : ");
            sb.Append(GetAbsolutePathFromUri());
            sb.AppendLine();

            sb.AppendLine("GetAssemblyPath() : ");
            sb.Append(GetAssemblyPath());
            sb.AppendLine();

            sb.AppendLine("GetAppDomainAppPath() : ");
            sb.Append(GetAppDomainAppPath());
            sb.AppendLine();

            sb.AppendLine("GetApplicationPhysicalPath() : ");
            sb.Append(GetApplicationPhysicalPath());
            sb.AppendLine();

            sb.AppendLine("GetWebAppRootDirectory() : ");
            sb.Append(GetWebAppRootDirectory());
            sb.AppendLine();

            return sb.ToString();
        }
    }
}