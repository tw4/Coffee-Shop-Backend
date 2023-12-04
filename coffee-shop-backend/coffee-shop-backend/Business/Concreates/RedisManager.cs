using coffee_shop_backend.Business.Abstracts;
using StackExchange.Redis;

namespace coffee_shop_backend.Business.Concreates;

public class RedisManager : IRedisServices
{

    private readonly ConnectionMultiplexer _redis;

    public RedisManager()
    {
        _redis = ConnectionMultiplexer.Connect("localhost:6379");
    }

    public string GetValue(string key)
    {
        IDatabase db = _redis.GetDatabase();
        return db.StringGet(key);
    }

    public void SetValue(string key, string value, TimeSpan expiry)
    {
        IDatabase db = _redis.GetDatabase();
        db.StringSet(key, value, expiry);
    }
}