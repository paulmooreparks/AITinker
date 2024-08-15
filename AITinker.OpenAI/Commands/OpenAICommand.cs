using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cliffer;

using AITinker.OpenAI.Services;

namespace AITinker.OpenAI.Commands;

[Command("openai", "Configuration and operations specific to OpenAI", aliases: ["oa"])]
internal class OpenAICommand {
    public async Task<int> Execute(
        Command command,
        InvocationContext context
        ) {
        return await command.Repl(ServiceUtility.GetServiceProvider(), context, new AITinker.Core.Services.SubCommandReplContext());
    }
}
