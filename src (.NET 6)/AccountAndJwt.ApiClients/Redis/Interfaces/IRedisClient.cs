namespace AccountAndJwt.ApiClients.Redis.Interfaces
{
    public interface IRedisClient : IDisposable
    {
        void SetObject<T>(String key, T obj);
        void SetObject<T>(String key, T obj, TimeSpan expiry);
        void SetString(String key, String str);
        void SetString(String key, String str, TimeSpan expiry);
        T GetObject<T>(String key);
        Boolean TryGetObject<T>(String key, out T obj);
        String GetString(String key);
        Boolean TryGetString(String key, out String str);
        void Invalidate(String key);
    }
}