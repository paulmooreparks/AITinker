namespace AITinker.OpenAI.Models;

public class OpenAIConfig {
    public OpenAIConfig() { 
        Settings = new SettingsModel();
        Defaults = new SettingsModel();
        Options = new OptionsModel();
    }

    public class SettingsModel {
        public string? ApiKey { get; set; }
        public string? ApiUrl { get; set; }
        public string? Model { get; set; }
    }

    public class OptionsModel {
        public string[]? ApiKey { get; set; }
        public string[]? ApiUrl { get; set; }
        public string[]? Model { get; set; }
    }

    public SettingsModel? Settings { get; set; }
    public SettingsModel? Defaults { get; set; }
    public OptionsModel? Options { get; set; }
}

