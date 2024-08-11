using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Cliffer;

using Microsoft.Extensions.Configuration;

namespace Quallm.OpenAI.Extensions;
public static class ClifferBuilderExtensions {
    public static IClifferBuilder AddOpenAICommands(this IClifferBuilder clifferBuilder, IConfiguration configuration, RootCommand rootCommand, IServiceProvider serviceProvider) {
        var assembly = Assembly.GetExecutingAssembly();
        clifferBuilder.AddCommands(assembly);
        return new ClifferBuilder(); 
    }
}
