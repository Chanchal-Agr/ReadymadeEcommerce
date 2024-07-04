using FrestyEcommerce.Shared.Dtos;
using FrestyEcommerce.Shared.Entities;

namespace FrestyEcommerce.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder(int userId);
        Task<ServiceResponse<List<OrderOverviewResponseDto>>> GetOrders();
        Task<ServiceResponse<OrderDetailsResponseDto>> GetOrderDetails(int orderId);
    }
}
