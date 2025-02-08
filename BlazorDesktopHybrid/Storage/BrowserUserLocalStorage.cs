using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorDesktopHybrid.Storage;

public class BrowserUserLocalStorage(ProtectedLocalStorage localStorage) : ILocalStorage
{
    public async ValueTask<UserStorageResult<TValue>> GetAsync<TValue>(string key)
    {
        try
        {
            var result = await localStorage.GetAsync<TValue>(key);
            return new UserStorageResult<TValue>(result.Success, result.Value);
        }
        catch (InvalidOperationException)
        {
        }

        return new UserStorageResult<TValue>(false, default);
    }

    public ValueTask SetAsync(string key, object value)
    {
        return localStorage.SetAsync(key, value);
    }

    public ValueTask DeleteAsync(string key)
    {
        return localStorage.DeleteAsync(key);
    }
}