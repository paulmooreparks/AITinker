using System.Reflection;
using Cliffer;

namespace Quallm.Cli.Services;

internal class QuallmReplContext : Cliffer.DefaultReplContext {
    public string Title => "quallm: Query Any LLM";

    public override string[] GetPopCommands() => [];

    public override string GetTitleMessage() {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Version? version = assembly.GetName().Version;
        string versionString = version?.ToString() ?? "Unknown";
        return $"{Title} v{versionString}";
    }
}

