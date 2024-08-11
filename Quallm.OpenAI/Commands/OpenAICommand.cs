using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cliffer;

namespace Quallm.OpenAI.Commands;

[Command("openai", "Configuration and operations specific to OpenAI")]
internal class OpenAICommand {
    public int Execute() {
        Console.WriteLine("OpenAI command");
        return Result.Success;
    }
}
