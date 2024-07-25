using FashionTimes.Shared.Dtos;
using FashionTimes.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace FashionTimes.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;
        private readonly IAuthService _authService;

        public OrderService(HttpClient http, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager, IAuthService authService)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
            _authService = authService;

        }

        public async Task<OrderDetailsResponseDto> GetOrderDetails(int orderId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponseDto>>($"api/order/{orderId}");
            return result.Data;
        }

        public async Task<List<OrderOverviewResponseDto>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponseDto>>>("api/order");
            return result.Data;
        }

        public async Task<bool> PlaceOrder()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _http.PostAsync("api/order/place-order", null);
                var value = (await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>()).Data;
                return value;
            }
           return false;
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
    }
}
