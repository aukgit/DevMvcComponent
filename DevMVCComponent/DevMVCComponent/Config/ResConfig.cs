using System.Collections.Generic;

namespace DevMvcComponent.Config
{
    /// <summary>
    /// Access resources from settings.
    /// </summary>
    public static class ResConfig
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

            var appIdConfig = Properties.Resources.ResourceManager.GetString(configName);
            if (appIdConfig != null && appIdConfig.Length >= 1)
            {
                var value = appIdConfig;
                ConfigList.Add(configName, value);
                return value;
            }
            return "";

        }



    }
}
