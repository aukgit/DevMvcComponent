using System;
using System.Configuration;
using System.IO;
using DevMvcComponent.Extensions;
namespace DevMvcComponent.Config
{
    /// <summary>
    /// Access app config.
    /// </summary>
    public static class AppConfig
    {

       
        /// <summary>
        /// Get current project's configuration's AppSettings value by id.
        /// </summary>
        /// <param name="key">Give a config name or key.</param>
        /// <returns>Returns value as string, if not found returns empty string. No exception.</returns>
        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            var appIdConfig = ConfigurationManager.AppSettings[key];
            if (appIdConfig != null && appIdConfig.Length >= 1)
            {
                var value = appIdConfig;
                return value;
            }
            return "";

        }

        /// <summary>
        /// Using settings's key returns an absolute path by adding program-files directory in front of the relative path.
        /// </summary>
        /// <param name="keyForRelativePath">Give a configuration key name to retrieve a relative path. Relative path should not have a slash in front of it. For example key value ="HelloWorld\ChildPath" will return as "C:\Program Files\HelloWorld\ChildPath" or "C:\Program Files()\HelloWorld\ChildPath"</param>
        /// <param name="isProgramFilesX86">If true then path would "C:\Program Files (x86)\" or "C:\Program Files\" based on operating system architecture. Or else it would be "C:\Program Files\".</param>
        /// <returns>Returns value as string, if not found returns empty string. No exception.</returns>
        public static string GetAbsolutePathWithProgramFiles(string keyForRelativePath, bool isProgramFilesX86 = true)
        {
            var relativePath = Get(keyForRelativePath);
            var programFilePath = "";
            if (isProgramFilesX86)
            {
                programFilePath = DirectoryExtension.GetProgramFilesX86Directory();
            }
            else
            {
                programFilePath = DirectoryExtension.GetProgramFilesDirectory();
            }
            return programFilePath + relativePath;
        }

        /// <summary>
        /// Using settings's key returns an absolute path by adding program-files directory in front of the relative path.
        /// </summary>
        /// <param name="config">External configuration.</param>
        /// <param name="keyForRelativePath">Give a configuration key name to retrieve a relative path. Relative path should not have a slash in front of it.</param>
        /// <param name="isProgramFilesX86">If true then path would "C:\Program Files (x86)\" or "C:\Program Files\" based on operating system architecture. Or else it would be "C:\Program Files\".</param>
        /// <returns>Returns value as string, if not found returns empty string. No exception.</returns>
        public static string GetAbsolutePathWithProgramFiles(Configuration config, string keyForRelativePath, bool isProgramFilesX86 = true)
        {
            var relativePath = Get(config, keyForRelativePath);
            var programFilePath = "";
            if (isProgramFilesX86)
            {
                programFilePath = DirectoryExtension.GetProgramFilesX86Directory();
            }
            else
            {
                programFilePath = DirectoryExtension.GetProgramFilesDirectory();
            }
            return programFilePath + relativePath;
        }

        /// <summary>
        /// Get external configuration's AppSettings value by id.
        /// </summary>
        /// <param name="config">External Configuration.</param>
        /// <param name="key"></param>
        /// <param name="defaultValue">Default to return if not found or not exist.</param>
        /// <returns>Returns value as string, if not found returns empty string. No exception.</returns>
        public static string Get(Configuration config, string key, string defaultValue = "")
        {
            if (config != null)
            {
                return Get(config.AppSettings, key, defaultValue);
            }
            return defaultValue;
        }
        /// <summary>
        /// Get external configuration's AppSettings value by id.
        /// </summary>
        /// <param name="appSettings">Pass any AppSettingsSection.</param>
        /// <param name="key"></param>
        /// <param name="defaultValue">Default to return if not found or not exist.</param>
        /// <returns>Returns value as string, if not found returns empty string. No exception.</returns>
        public static string Get(AppSettingsSection appSettings, string key, string defaultValue = "")
        {
            if (appSettings != null)
            {
                var appIdConfig = appSettings.Settings[key];
                if (appIdConfig != null)
                {
                    var value = appIdConfig.Value;
                    return value;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Get current project's configuration's appsettings value by id.
        /// Throw exception if can't convert to int.
        /// </summary>
        /// <param name="configName">Give a config name</param>
        /// <param name="defaultValue">Default value to return when key is not found.</param>
        /// <returns>Returns value as string, if not found returns empty string. No execption.</returns>
        public static int GetAsInt(string configName, int defaultValue = 0)
        {
            var value = Get(configName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return int.Parse(value);
        }

        /// <summary>
        /// Get current project's configuration's appsettings value by id.
        /// Throw exception if can't convert to long.
        /// </summary>
        /// <param name="configName">Give a config name</param>
        /// <param name="defaultValue">Default value to return when key is not found.</param>
        /// <returns>Returns value as string, if not found returns empty string. No execption.</returns>
        public static long GetAsLong(string configName, long defaultValue = 0)
        {
            var value = Get(configName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return long.Parse(value);
        }
        /// <summary>
        /// Get config value by key.
        /// Throw exception if can't convert to decimal.
        /// </summary>
        /// <param name="configName">Give a config name</param>
        /// <param name="defaultValue">Default value to return when key is not found.</param>
        /// <returns>Returns value as string, if not found returns empty string. No execption.</returns>
        public static decimal GetAsDecimal(string configName, decimal defaultValue = 0)
        {
            var value = Get(configName);
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }
            return decimal.Parse(value);
        }

        /// <summary>
        /// Get the external config file's configuration.
        /// If file not found the return null.
        /// </summary>
        /// <param name="fileLocation">Location of the config file.</param>
        /// <returns></returns>
        public static Configuration GetExternalConfig(string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                var configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = fileLocation
                };
                return ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            }
            return null;
        }

        /// <summary>
        /// Get the external configuration file's AppSetting.
        /// If file not found the return null.
        /// </summary>
        /// <param name="fileLocation">Location of the config file.</param>
        /// <returns></returns>
        public static AppSettingsSection GetExternalConfigAppSetting(string fileLocation)
        {
            var config = GetExternalConfig(fileLocation);
            if (config != null)
            {
                return config.AppSettings;
            }
            return null;
        }

        /// <summary>
        /// Get the external configuration file's ConnectionStrings.
        /// If file not found the return null.
        /// </summary>
        /// <param name="fileLocation">Location of the config file.</param>
        /// <returns></returns>
        public static ConnectionStringsSection GetExternalConfigConnectionStrings(string fileLocation)
        {
            var config = GetExternalConfig(fileLocation);
            if (config != null)
            {
                return config.ConnectionStrings;
            }
            return null;
        }

        /// <summary>
        /// Update given configuration's AppSettings.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public static void UpdateAppSettings(Configuration configuration, string key, string newValue)
        {
            var appSettings = configuration.AppSettings;
            appSettings.Settings.Remove(key);
            appSettings.Settings.Add(key, newValue);
            configuration.Save(ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// Update current project's AppSettings.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public static void UpdateAppSettings(string key, string newValue)
        {
            ConfigurationManager.AppSettings.Set(key, newValue);
        }

    }
}
