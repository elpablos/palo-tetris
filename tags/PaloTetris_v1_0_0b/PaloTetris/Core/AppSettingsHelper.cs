using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PaloTetris.Core
{
    /// <summary>
    /// Pomocna trida pro praci s AppSettings.
    /// </summary>
    public static class AppSettingsHelper
    {
        /// <summary>
        /// Cteni textove hodnoty z AppSettings.
        /// </summary>
        /// <param name="name">klic</param>
        /// <returns>hodnota</returns>
        public static string ReadProperty(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// Pokus o precteni textove hodnoty z AppSettings.
        /// </summary>
        /// <param name="name">klic</param>
        /// <param name="prop">vystupni hodnota, pokud se cteni povede</param>
        /// <returns>true, pokud se povedlo</returns>
        public static bool TryReadProperty(string name, out string prop)
        {
            prop = string.Empty;
            try
            {
                prop = ConfigurationManager.AppSettings[name];
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Cteni ciselne hodnoty z AppSettings.
        /// </summary>
        /// <param name="name">klic</param>
        /// <returns>hodnota</returns>
        public static int ReadIntProperty(string name)
        {
            return Int32.Parse(ConfigurationManager.AppSettings[name]);
        }

        /// <summary>
        /// Pokus o precteni ciselne hodnoty z AppSettings.
        /// </summary>
        /// <param name="name">klic</param>
        /// <param name="prop">vystupni hodnota, pokud se cteni povede</param>
        /// <returns>true, pokud se povedlo</returns>
        public static bool TryReadProperty(string name, out int prop)
        {
            return Int32.TryParse(ConfigurationManager.AppSettings[name], out prop);
        }

        /// <summary>
        /// Zapis hodnoty do config souboru.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetProperty(string name, string value)
        {
            /// TODO nefunguje ?! funguje, ale jen v pameti
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            //config.AppSettings.Settings.Clear();
            config.AppSettings.Settings[name].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(Constants.AppSettings);
        }
    }
}
