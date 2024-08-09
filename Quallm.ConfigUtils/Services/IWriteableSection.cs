using Microsoft.Extensions.Options;

namespace Quallm.ConfigUtils.Services;

public interface IWriteableSection<out T> : IOptions<T> where T : class, new() {
    void Update(Action<T> applyChanges);
}

