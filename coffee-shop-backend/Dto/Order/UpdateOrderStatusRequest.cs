using coffee_shop_backend.Entitys.Concreates;

namespace coffee_shop_backend.Dto.Order;

public class UpdateOrderStatusRequest
{
    public EnumOrderStatus Status { get; set; }
}