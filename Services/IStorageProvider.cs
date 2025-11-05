using System.Text.Json;

namespace Pitzam.Services
{
    public interface IStorageProvider
    {
        Task SetItemAsync<T>(string key, T value, JsonSerializerOptions? options = null);
        Task<T?> GetItemAsync<T>(string key, JsonSerializerOptions? options = null);
        Task RemoveItemAsync(string key);
    }
}


