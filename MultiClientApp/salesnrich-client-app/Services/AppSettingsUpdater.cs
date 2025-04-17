using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SNR_ClientApp.Properties;
using SNR_ClientApp.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Configuration;

namespace SNR_ClientApp.Services
{// Code to change the fullURL of appsettings...Appsetting is XML file....
    public class AppSettingsUpdater
    {
        private readonly string _appSettingsPath;
        private static IConfigurationRoot configuration;

        public static IConfigurationRoot GetConfiguration()
        {
            if (configuration == null)
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            }
            else
            {
                configuration.Reload();
            }

            return configuration;
        }
        public AppSettingsUpdater(string appSettingsPath)
        {
            _appSettingsPath = appSettingsPath;
        }
        public void UpdateFullUrl(string newUrl)
        {
            try
            {
                // Update the FullURL setting in the application configuration
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = config.AppSettings.Settings;

                if (appSettings["FullURL"] != null)
                {
                    appSettings["FullURL"].Value = newUrl; // Update existing value
                }
                else
                {
                    appSettings.Add("FullURL", newUrl); // Add new key if it doesn't exist
                }

                // Update the external XML file, assuming _appSettingsPath is defined
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_appSettingsPath);
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

                if (appSettingsNode != null)
                {
                    // Locate or add the FullURL setting in the XML document
                    XmlNode fullUrlNode = appSettingsNode.SelectSingleNode("add[@key='FullURL']");
                    if (fullUrlNode != null)
                    {
                        fullUrlNode.Attributes["value"].Value = newUrl; // Update the value
                    }
                    else
                    {
                        // Add FullURL node if it doesn't exist
                        XmlElement newSetting = xmlDoc.CreateElement("add");
                        newSetting.SetAttribute("key", "FullURL");
                        newSetting.SetAttribute("value", newUrl);
                        appSettingsNode.AppendChild(newSetting);
                    }

                    // Save changes to the XML document
                    xmlDoc.Save(_appSettingsPath);
                }

                // Save configuration changes to the application settings
                config.Save(ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");

                // Update in-memory properties and file update
                ApplicationProperties.properties["service.full.url"] = newUrl;
                ApplicationProperties.updatePropertiesFile();

                // Optional: Rebuild configuration using ConfigurationBuilder
                string configPath = Path.Combine(AppContext.BaseDirectory, "App.config");
                var configBuilder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddXmlFile(configPath, optional: false, reloadOnChange: true);

                try
                {
                    var configuration = configBuilder.Build();
                    LogManager.WriteLog("Configuration updated successfully.");
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog($"Error loading configuration: {ex.Message}");
                    Console.WriteLine($"Error loading configuration: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog("An error occurred while updating FullURL."+ex.Message);
                throw new Exception("Failed to update FullURL.", ex);
            }
        }




    }
}

