
using System.Text.Json;
using StackExchange.Redis;

namespace RedisCaching.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cacheDb = redis.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _cacheDb.StringGet(key);
            if (value.HasValue)
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public object RemoveData(string key)
        {
            var isExist = _cacheDb.KeyExists(key);
            if (isExist)
            {
                return _cacheDb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expiration)
        {
            var expireTime = expiration.DateTime.Subtract(DateTime.Now);

            // Trả về true nếu set thành công, ngược lại trả về false. Nếu key đã tồn tại thì sẽ bị ghi đè.
            return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expireTime);
        }
    }
}