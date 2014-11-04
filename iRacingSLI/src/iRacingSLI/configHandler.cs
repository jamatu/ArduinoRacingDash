using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingSLI
{
    class configHandler
    {
        Action<String> console;

        public configHandler(Action<String> callConsole)
        {
            console = callConsole;
        }

        public String readSetting(String Key, String defaultValue)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[Key] ?? "NotFound";
                if (result == "NotFound")
                {
                    console("Key '" + Key + "' not found. Creating key....");
                    writeSetting(Key, defaultValue);
                    result = defaultValue;
                }
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                console("Error Reading Config Key: " + Key);
                return "0";
            }
        }

        public void writeSetting(String Key, String Value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[Key] == null)
                {
                    settings.Add(Key, Value);
                }
                else
                {
                    settings[Key].Value = Value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                console("Error Writing Key: "+ Key + " to Config File");
            }
        }
    }
}
