using System.CommandLine;
using System.CommandLine.Invocation;
using Cliffer;

namespace AITinker.Cli.Commands;

[RootCommand("ait, the AI Tinker CLI")]
internal class RootCommand {
    public async Task<int> Execute(Command command, IServiceProvider serviceProvider, InvocationContext context) {
        return await command.Repl(serviceProvider, context, new AITinker.Core.Services.AITinkerReplContext());
    }
}
