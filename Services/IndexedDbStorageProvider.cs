using System.Text.Json;
using Microsoft.JSInterop;

namespace Pitzam.Services
{
    public class IndexedDbStorageProvider : IStorageProvider
    {
        private readonly IJSRuntime _js;
        private static readonly JsonSerializerOptions DefaultOptions = new(JsonSerializerDefaults.Web);

        public IndexedDbStorageProvider(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SetItemAsync<T>(string key, T value, JsonSerializerOptions? options = null)
        {
            var json = JsonSerializer.Serialize(value, options ?? DefaultOptions);
            await _js.InvokeVoidAsync("pitzamIdb.set", key, json);
        }

        public async Task<T?> GetItemAsync<T>(string key, JsonSerializerOptions? options = null)
        {
            var json = await _js.InvokeAsync<string?>("pitzamIdb.get", key);
            if (string.IsNullOrWhiteSpace(json)) return default;
            try
            {
                return JsonSerializer.Deserialize<T>(json!, options ?? DefaultOptions);
            }
            catch
            {
                return default;
            }
        }

        public async Task RemoveItemAsync(string key)
        {
            await _js.InvokeVoidAsync("pitzamIdb.remove", key);
        }
    }
}


