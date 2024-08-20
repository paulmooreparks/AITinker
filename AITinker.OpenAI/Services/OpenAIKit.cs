using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AITinker.Core.Services;

namespace AITinker.OpenAI.Services;

public class OpenAIKit : IKitConfiguration {
    private const string _name = "OpenAI";

    public string Name => _name;

    public object GetDefaults() {
        return new { };
    }

    public Dictionary<string, List<string>> GetOptions() {
        throw new NotImplementedException();
    }

    public object GetSettings(Dictionary<string, string> values) {
        return new { };
    }
}
