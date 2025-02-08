namespace BlazorDesktopHybrid.Storage;


public readonly struct UserStorageResult<TValue>(bool success, TValue? value)
{
    public bool Success { get; } = success;
    public TValue? Value { get; } = value;
}

public interface IUserStorage
{
    ValueTask<UserStorageResult<TValue>> GetAsync<TValue>(string key);
    ValueTask SetAsync(string key, object value);
    ValueTask DeleteAsync(string key);
}

public interface ISessionStorage : IUserStorage
{
}

public interface ILocalStorage : IUserStorage
{
}
