namespace coffee_shop_backend.Business.Abstracts;

public interface IRedisServices
{
    string GetValue(string key);
    void SetValue(string key, string value, TimeSpan expiry);
}