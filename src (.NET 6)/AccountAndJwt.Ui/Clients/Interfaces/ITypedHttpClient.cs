namespace AccountAndJwt.Ui.Clients.Interfaces
{
    public interface ITypedHttpClient
    {
        Task<HttpResponseMessage> GetAsync(String uri, String accessToken);
        Task<HttpResponseMessage> PostAsync<T>(String uri, T obj, String accessToken);
    }
}