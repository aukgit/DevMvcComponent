using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;

namespace DevMvcComponent.Config
{

    /// <summary>
    /// Access web config.
    /// </summary>
    public static class WebConfig
    {
        private static readonly Dictionary<string, string> ConfigList = new Dictionary<string, string>(30);
        /// <summary>
        /// Get config value by configName
        /// </summary>
        /// <param name="configName">Give a config name</param>
        /// <returns>Returns value as string, if not found returns empty string. No execption.</returns>
        public static string Get(string configName)
        {
            if (string.IsNullOrEmpty(configName))
            {
                return "";
            }
            if (ConfigList.ContainsKey(configName))
            {
                // if cache exist
                var cachedConfigValue = ConfigList[configName];
                return cachedConfigValue;
            }

            var appIdConfig = WebConfigurationManager.AppSettings[configName];
            if (appIdConfig != null && appIdConfig.Length >= 1)
            {
                var value = appIdConfig.FirstOrDefault().ToString();
                ConfigList.Add(configName, value);
                return value;
            }
            return "";

        }

    }
}
