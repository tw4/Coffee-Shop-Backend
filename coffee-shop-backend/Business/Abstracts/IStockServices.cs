using coffee_shop_backend.Dto.Stock;

namespace coffee_shop_backend.Business.Abstracts;

public interface IStockServices: ICrudServices<AddStockRequest, UpdateStockRequest>
{
}