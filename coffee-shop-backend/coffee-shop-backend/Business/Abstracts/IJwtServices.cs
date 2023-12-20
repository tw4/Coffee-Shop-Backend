namespace coffee_shop_backend.Business.Abstracts;

public interface IJwtServices
{
   public string GenerateJwtToken(long Id, string email);
   public long GetUserIdFromToken(string token);
   public bool IsTokenValid(string token);
   public string GetUserEmailFromToken(string token);
}