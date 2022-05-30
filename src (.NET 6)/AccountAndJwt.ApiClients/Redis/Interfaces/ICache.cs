namespace AccountAndJwt.ApiClients.Redis.Interfaces
{
    public interface ICache : IRedisClient
    {
        Boolean IsCachingEnabled { get; }

        new void SetString(String key, String str, TimeSpan expiry);
    }
}