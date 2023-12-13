using coffee_shop_backend.Business.Abstracts;
using StackExchange.Redis;

namespace coffee_shop_backend.Business.Concreates;

public class RedisManager : IRedisServices
{

    private readonly ConnectionMultiplexer _redis;
    private readonly Logger<RedisManager> _logger;

    public RedisManager(Logger<RedisManager> logger)
    {
        _redis = ConnectionMultiplexer.Connect("localhost:6379");
        _logger = logger;
    }

    public string GetValue(string key)
    {
        IDatabase db = _redis.GetDatabase();
        _logger.LogInformation("RedisManager.GetValue() method is called.");
        return db.StringGet(key);
    }

    public void SetValue(string key, string value, TimeSpan expiry)
    {
        IDatabase db = _redis.GetDatabase();
        _logger.LogInformation("RedisManager.SetValue() method is called.");
        db.StringSet(key, value, expiry);
    }
}