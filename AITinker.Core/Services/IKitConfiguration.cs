namespace AITinker.Core.Services;

public interface IKitConfiguration {
    string Name { get; }
    object GetSettings(Dictionary<string, string> values);
    object GetDefaults();
    Dictionary<string, List<string>> GetOptions();
}
