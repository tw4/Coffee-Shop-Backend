using coffee_shop_backend.Business.Abstracts;
using StackExchange.Redis;

namespace coffee_shop_backend.Business.Concreates;

public class RedisServices : IRedisServices
{

    private readonly ConnectionMultiplexer _redis;
    private readonly Logger<RedisServices> _logger;

    public RedisServices(Logger<RedisServices> logger)
    {
        _redis = ConnectionMultiplexer.Connect("localhost:6379");
        _logger = logger;
    }

    public string GetValue(string key)
    {
        IDatabase db = _redis.GetDatabase();
        _logger.LogInformation("RedisServices.GetValue() method is called.");
        return db.StringGet(key);
    }

    public void SetValue(string key, string value, TimeSpan expiry)
    {
        IDatabase db = _redis.GetDatabase();
        _logger.LogInformation("RedisServices.SetValue() method is called.");
        db.StringSet(key, value, expiry);
    }
}