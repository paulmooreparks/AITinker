using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

namespace Quallm.Cli.Services;

public interface IWriteableSection<out T> : IOptions<T> where T : class, new() {
    void Update(Action<T> applyChanges);
}

