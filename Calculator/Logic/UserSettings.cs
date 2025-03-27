using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace Calculator.Logic
{
    public class UserSettings
    {
        public string CalculationMode { get; set; } = "AfterEqual"; 
        public string Mode { get; set; } = "Standard";
        public int NumericBase { get; set; } = 10; 
        public bool DigitGrouping { get; set; } = false;

        private static readonly string SettingsFilePath = "user_settings.json";
        public void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error saving settings: " + ex.Message);
            }
        }
        public static UserSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);

                    Console.WriteLine(" JSON Read from file: " + json);

                    UserSettings settings = JsonConvert.DeserializeObject<UserSettings>(json);

                    if (settings != null)
                        return settings;
                    else
                        Console.WriteLine(" Deserialization failed. Returning default settings.");
                }
                else
                {
                    Console.WriteLine("Settings file not found. Using default settings.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error loading settings: " + ex.Message);
            }
            return new UserSettings();
        }

    }
}
