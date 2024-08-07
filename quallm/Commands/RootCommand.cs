using System.CommandLine;
using System.CommandLine.Invocation;
using Cliffer;

namespace Quallm.Cli.Commands;

[RootCommand("quallm, Query Any LLM")]
internal class RootCommand {
    public async Task<int> Execute(Command command, IServiceProvider serviceProvider, InvocationContext context) {
        return await command.Repl(serviceProvider, context, new Services.QuallmReplContext());
    }
}
