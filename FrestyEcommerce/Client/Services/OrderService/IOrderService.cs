using FrestyEcommerce.Shared.Dtos;

namespace FrestyEcommerce.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task<bool> PlaceOrder();
        Task<List<OrderOverviewResponseDto>> GetOrders();
        Task<OrderDetailsResponseDto> GetOrderDetails(int orderId);
    }
}
