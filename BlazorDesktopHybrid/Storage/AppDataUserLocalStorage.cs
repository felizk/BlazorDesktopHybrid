using System.Text.Json;

namespace BlazorDesktopHybrid.Storage;


public class AppDataUserLocalStorage(string folder) : ILocalStorage
{
    public async ValueTask<UserStorageResult<TValue>> GetAsync<TValue>(string key)
    {
        var filePath = KeyToFile(key);

        if (!string.IsNullOrWhiteSpace(key))
        {
            if (File.Exists(filePath))
            {
                try
                {
                    var str = await File.ReadAllTextAsync(filePath);
                    var value = JsonSerializer.Deserialize<TValue>(str);
                    return new UserStorageResult<TValue>(true, value);
                }
                catch (JsonException)
                {
                    await DeleteAsync(key);
                }
            }
        }

        return new UserStorageResult<TValue>(false, default);
    }

    public ValueTask SetAsync(string key, object value)
    {
        var filePath = KeyToFile(key);
        var obj = JsonSerializer.Serialize(value);
        
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        
        File.WriteAllText(filePath, obj);
        return ValueTask.CompletedTask;
    }

    public ValueTask DeleteAsync(string key)
    {
        var filePath = KeyToFile(key);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        
        return ValueTask.CompletedTask;
    }

    private string KeyToFile(string key)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            key = key.Replace(c, '_');
        }

        return Path.Combine(folder, key) + ".json";
    }
    
}