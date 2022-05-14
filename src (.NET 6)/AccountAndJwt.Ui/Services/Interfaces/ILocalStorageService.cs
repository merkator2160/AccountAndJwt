namespace AccountAndJwt.Ui.Services.Interfaces
{
    internal interface ILocalStorageService
    {
        Task<T> GetObjectAsync<T>(String key);
        Task SetObjectAsync<T>(String key, T value);
        Task RemoveAsync(String key);
    }
}