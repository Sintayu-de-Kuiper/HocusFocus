using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;


namespace HocusFocus.Core
{
    public class ConfigurationManager
    {
        private const string ConfigFilePath = "config.json";

        // Method to load the blacklist configuration from a JSON file
        public BlacklistConfig LoadConfiguration()
        {
            try
            {
                if (!File.Exists(ConfigFilePath))
                {
                    // Return a new configuration if the file does not exist
                    return new BlacklistConfig { BlockedApps = new List<string>() };
                }

                // Read the JSON file
                string json = File.ReadAllText(ConfigFilePath);
                // Deserialize the JSON to the BlacklistConfig object
                return JsonSerializer.Deserialize<BlacklistConfig>(json) ?? new BlacklistConfig { BlockedApps = new List<string>() };
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., file not found, deserialization error)
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                return new BlacklistConfig { BlockedApps = new List<string>() };
            }
        }

        // Method to save the current blacklist configuration to a JSON file
        public void SaveConfiguration(BlacklistConfig config)
        {
            try
            {
                // Serialize the BlacklistConfig object to JSON
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                // Write the JSON to the file
                File.WriteAllText(ConfigFilePath, json);
                Debug.WriteLine("Successfully saved the configuration!");
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., unable to write to file)
                Console.WriteLine($"Error saving configuration: {ex.Message}");
            }
        }
    }
}
