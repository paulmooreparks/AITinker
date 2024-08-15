using Microsoft.Extensions.Options;

namespace AITinker.Core.Services;

public interface IWriteableSection<out T> : IOptions<T> where T : class, new() {
    void Update(Action<T> applyChanges);
}

