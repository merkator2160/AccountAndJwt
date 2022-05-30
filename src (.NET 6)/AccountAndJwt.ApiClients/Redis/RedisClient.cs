using AccountAndJwt.ApiClients.Redis.Config;
using AccountAndJwt.ApiClients.Redis.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AccountAndJwt.ApiClients.Redis
{
    public class RedisClient : ICache
    {
        private readonly RedisClientConfig _config;
        private readonly ConnectionMultiplexer _connection;


        public RedisClient(RedisClientConfig config)
        {
            _config = config;
            _connection = ConnectionMultiplexer.Connect(config.ConnectionString);
        }



        // ICache /////////////////////////////////////////////////////////////////////////////////
        public Boolean IsCachingEnabled => _config.IsCachingEnabled;

        void ICache.SetString(String key, String str, TimeSpan expiry)
        {
            if (_config.IsCachingEnabled)
            {
                _connection.GetDatabase().StringSet(key, str, expiry);
            }
        }


        // IRedisClient ///////////////////////////////////////////////////////////////////////////
        public void SetString(String key, String str, TimeSpan expiry)
        {
            _connection.GetDatabase().StringSet(key, str, expiry);
        }
        public void SetString(String key, String str)
        {
            SetString(key, str, TimeSpan.FromSeconds(_config.DefaultExpirySec));
        }
        public void SetObject<T>(String key, T obj, TimeSpan expiry)
        {
            SetString(key, JsonConvert.SerializeObject(obj), expiry);
        }
        public void SetObject<T>(String key, T obj)
        {
            SetString(key, JsonConvert.SerializeObject(obj));
        }
        public String GetString(String key)
        {
            return _connection.GetDatabase().StringGet(key);
        }
        public T GetObject<T>(String key)
        {
            return JsonConvert.DeserializeObject<T>(GetString(key));
        }
        public Boolean TryGetString(String key, out String str)
        {
            str = GetString(key);
            return !String.IsNullOrEmpty(str);
        }
        public Boolean TryGetObject<T>(String key, out T obj)
        {
            if (TryGetString(key, out var str))
            {
                obj = JsonConvert.DeserializeObject<T>(GetString(key));
                return true;
            }

            obj = default(T);
            return false;
        }


        // IDisposable ////////////////////////////////////////////////////////////////////////////
        public void Dispose()
        {
            _connection?.Dispose();
        }


        // Common /////////////////////////////////////////////////////////////////////////////////
        public void Invalidate(String key)
        {
            _connection.GetDatabase().KeyDelete(key);
        }
    }
}