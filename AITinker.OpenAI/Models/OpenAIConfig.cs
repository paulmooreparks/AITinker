namespace AITinker.OpenAI.Models;

public class OpenAIConfig {
    public OpenAIConfig() { 
        Defaults = new OpenAIDefaults();
        Options = new OpenAIOptions();
    }

    public OpenAIDefaults? Defaults { get; set; }
    public OpenAIOptions? Options { get; set; }
}

public class OpenAISettings : AITinker.Core.Models.ITinkerSettings {
    public string? ApiKey { get; set; }
    public string? ApiUrl { get; set; }
    public string? Model { get; set; }
    public string? SystemContent { get; set; }
    public double? Temperature { get; set; }
}

public class OpenAIDefaults {
    public string? ApiUrl { get; set; }
    public string? Model { get; set; }
    public string? SystemContent { get; set; }
    public double? Temperature { get; set; }
}

public class OpenAIOptions {
    public string[]? ApiUrl { get; set; }
    public string[]? Model { get; set; }
}

