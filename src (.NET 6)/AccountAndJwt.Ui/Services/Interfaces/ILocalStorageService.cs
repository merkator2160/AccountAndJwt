namespace AccountAndJwt.Ui.Services.Interfaces
{
    internal interface ILocalStorageService
    {
        Task<T> GetItemAsync<T>(String key);
        Task SetItemAsync<T>(String key, T value);
        Task RemoveItemAsync(String key);
    }
}