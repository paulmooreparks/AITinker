using System.Reflection;

namespace AITinker.Core.Services;

public class AITinkerReplContext : Cliffer.DefaultReplContext {
    public string Title => "ait, The AI Tinker CLI";

    public override string[] GetPopCommands() => ["up"];

    public override string GetTitleMessage() {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Version? version = assembly.GetName().Version;
        string versionString = version?.ToString() ?? "Unknown";
        return $"{Title} v{versionString}";
    }
}

