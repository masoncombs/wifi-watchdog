using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace WifiWatchdogLauncher
{
    public static class SettingsReader
    {
        public static bool GetDarkMode()
        {
            try
            {
                // Find the user.config file for WifiWatchdogApp
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string configDir = Path.Combine(appData, "WifiWatchdogApp");
                if (!Directory.Exists(configDir)) return false;
                var files = Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories);
                if (files.Length == 0) return false;
                var config = XDocument.Load(files[0]);
                var darkModeElem = config.Descendants("setting")
                    .FirstOrDefault(e => (string)e.Attribute("name") == "DarkMode");
                if (darkModeElem == null) return false;
                var valueElem = darkModeElem.Element("value");
                if (valueElem == null) return false;
                return bool.TryParse(valueElem.Value, out bool result) && result;
            }
            catch { return false; }
        }

        public static bool? TryGetDarkMode()
        {
            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string configDir = Path.Combine(appData, "WifiWatchdogApp");
                if (!Directory.Exists(configDir)) return null;
                var files = Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories);
                if (files.Length == 0) return null;
                var config = XDocument.Load(files[0]);
                var darkModeElem = config.Descendants("setting")
                    .FirstOrDefault(e => (string)e.Attribute("name") == "DarkMode");
                if (darkModeElem == null) return null;
                var valueElem = darkModeElem.Element("value");
                if (valueElem == null) return null;
                if (bool.TryParse(valueElem.Value, out bool result))
                    return result;
                return null;
            }
            catch { return null; }
        }

        public static bool GetAutoRunPromptShown()
        {
            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string configDir = Path.Combine(appData, "WifiWatchdogApp");
                if (!Directory.Exists(configDir)) return false;
                var files = Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories);
                if (files.Length == 0) return false;
                var config = XDocument.Load(files[0]);
                var elem = config.Descendants("setting")
                    .FirstOrDefault(e => (string)e.Attribute("name") == "AutoRunPromptShown");
                if (elem == null) return false;
                var valueElem = elem.Element("value");
                if (valueElem == null) return false;
                return bool.TryParse(valueElem.Value, out bool result) && result;
            }
            catch { return false; }
        }

        public static bool? TryGetAutoRunPromptShown()
        {
            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string configDir = Path.Combine(appData, "WifiWatchdogApp");
                if (!Directory.Exists(configDir)) return null;
                var files = Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories);
                if (files.Length == 0) return null;
                var config = XDocument.Load(files[0]);
                var elem = config.Descendants("setting")
                    .FirstOrDefault(e => (string)e.Attribute("name") == "AutoRunPromptShown");
                if (elem == null) return null;
                var valueElem = elem.Element("value");
                if (valueElem == null) return null;
                if (bool.TryParse(valueElem.Value, out bool result))
                    return result;
                return null;
            }
            catch { return null; }
        }

        public static void SetAutoRunPromptShown(bool value)
        {
            try
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string configDir = Path.Combine(appData, "WifiWatchdogApp");
                if (!Directory.Exists(configDir)) return;
                var files = Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories);
                if (files.Length == 0) return;
                var config = XDocument.Load(files[0]);
                var elem = config.Descendants("setting")
                    .FirstOrDefault(e => (string)e.Attribute("name") == "AutoRunPromptShown");
                if (elem == null)
                {
                    // Add the setting if it doesn't exist
                    var settingsSection = config.Descendants("userSettings").FirstOrDefault();
                    if (settingsSection == null) return;
                    var settingsGroup = settingsSection.Elements().FirstOrDefault();
                    if (settingsGroup == null) return;
                    elem = new XElement("setting",
                        new XAttribute("name", "AutoRunPromptShown"),
                        new XAttribute("serializeAs", "String"),
                        new XElement("value", value.ToString())
                    );
                    settingsGroup.Add(elem);
                }
                else
                {
                    var valueElem = elem.Element("value");
                    if (valueElem == null)
                    {
                        valueElem = new XElement("value", value.ToString());
                        elem.Add(valueElem);
                    }
                    else
                    {
                        valueElem.Value = value.ToString();
                    }
                }
                config.Save(files[0]);
            }
            catch { }
        }
    }
}
