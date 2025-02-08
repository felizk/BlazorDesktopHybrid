namespace BlazorDesktopHybrid.Storage;


public class InMemoryUserLocalStorage : ILocalStorage
{
    public ValueTask<UserStorageResult<TValue>> GetAsync<TValue>(string key)
    {
        if (_data.TryGetValue(key, out var value))
        {
            return ValueTask.FromResult(new UserStorageResult<TValue>(true, (TValue)value));
        }

        return ValueTask.FromResult(new UserStorageResult<TValue>(false, default));
    }

    public ValueTask SetAsync(string key, object value)
    {
        _data[key] = value;
        return ValueTask.CompletedTask;
    }

    public ValueTask DeleteAsync(string key)
    {
        _data.Remove(key);
        return ValueTask.CompletedTask;
    }

    private Dictionary<string, object> _data = new();
}