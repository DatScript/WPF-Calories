using System.IO;
using System.Text.Json;
using System.Windows;

namespace Calories.App.Helper;
public class ConfigHelper
{
    string ConfigFile = $"{Utils.AppFolder}\\ProgramData\\config\\config.json";
    private static ConfigHelper _Instance = new();
    public static ConfigHelper Instance { get { return _Instance; } set { _Instance = value; } }

    public ModelConfig Config { get; set; } = new();

    public ConfigHelper()
    {
        Parse();
    }

    public void Parse()
    {
        if (!File.Exists(ConfigFile))
        {
            Config = DefaultConfig();
        }
        else
        {
            string ConfigText = File.ReadAllText(ConfigFile);
            if (string.IsNullOrEmpty(ConfigText)) { Config = DefaultConfig(); }
            else
            {
                var _Config = JsonSerializer.Deserialize<ModelConfig>(ConfigText);
                if (_Config == null)
                {
                    MessageBox.Show("Cannot parse config file, loaded default config.");
                    Config = DefaultConfig();
                }
                else
                    Config = _Config;
            }
        }
        Save();
    }

    public void Save()
    {
        string ConfigText = JsonSerializer.Serialize(Config);
        Directory.CreateDirectory(Config.ProgramDataPath);
        Directory.CreateDirectory(Config.RecordPath);
        Directory.CreateDirectory(Path.Combine(Config.ProgramDataPath, "config"));
        File.WriteAllText(ConfigFile, ConfigText);
    }

    private ModelConfig DefaultConfig()
    {
        ModelConfig Default = new();
        Default.DbHost = "127.0.0.1";
        Default.DbUsername = "postgres";
        Default.DbPassword = "Sbwrm2580*";
        Default.DbName = "DB_INTERCOM";
        Default.ProgramDataPath = $"{Utils.AppFolder}\\ProgramData";
        Default.RecordPath = $"{Utils.AppFolder}\\Record";
        return Default;
    }
}

public class ModelConfig
{
    public string DbHost { get; set; } = string.Empty;
    public string DbUsername { get; set; } = string.Empty;
    public string DbPassword { get; set; } = string.Empty;
    public string DbName { get; set; } = string.Empty;
    public string ProgramDataPath { get; set; } = string.Empty;
    public string RecordPath { get; set; } = string.Empty;
}