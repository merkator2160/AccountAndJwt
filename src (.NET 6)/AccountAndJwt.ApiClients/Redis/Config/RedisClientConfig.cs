namespace AccountAndJwt.ApiClients.Redis.Config
{
    public class RedisClientConfig
    {
        public String ConnectionString { get; set; }
        public Int32 DefaultExpirySec { get; set; }
        public Boolean IsCachingEnabled { get; set; }
    }
}