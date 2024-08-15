using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AITinker.Core.Services;

public class SubCommandReplContext : AITinkerReplContext {
#if false
    public override string[] GetPopCommands() => ["up"];
#endif

    public override string GetEntryMessage() {
        return $@"Type 'help' for a list of sub-commands, 'up' to return to previous, or 'exit' to exit.";
    }
}
