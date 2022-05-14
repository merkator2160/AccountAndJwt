using AccountAndJwt.Ui.Services.Interfaces;
using Microsoft.JSInterop;
using System.Text.Json;

namespace AccountAndJwt.Ui.Services
{
    internal class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;


        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }


        // ILocalStorageService ///////////////////////////////////////////////////////////////////
        public async Task<T> GetItemAsync<T>(String key)
        {
            var json = await _jsRuntime.InvokeAsync<String>("localStorage.getItem", key);
            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }
        public async Task SetItemAsync<T>(String key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
        }
        public async Task RemoveItemAsync(String key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}