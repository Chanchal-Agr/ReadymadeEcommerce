using FrestyEcommerce.Client.Pages;
using FrestyEcommerce.Shared.Dtos;
using FrestyEcommerce.Shared.Entities;
using System.Security.Claims;
using static MudBlazor.CategoryTypes;
using static MudBlazor.FilterOperator;

namespace FrestyEcommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext context, ICartService cartService, IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsResponseDto>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponseDto>();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            var prices = await this._context.ProductVariants.Where(x => order.OrderItems.Select(y => y.ProductId).Contains(x.ProductId) && order.OrderItems.Select(y => y.ProductTypeId).Contains(x.ProductTypeId)).Select(x => new ProductVariant
            {
                ProductTypeId = x.ProductTypeId,
                ProductId = x.ProductId,
                OriginalPrice = x.OriginalPrice
            }).ToListAsync();

            if (order == null)
            {
                response.Success = false;
                response.Message = "Order not found.";
                return response;
            }
            string number = order.Id.ToString().PadLeft(4, '0');
            number = $"RM{number}";
            var orderDetailsResponse = new OrderDetailsResponseDto
            {
                OrderNumber = number,
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductResponseDto>()
            };

            order.OrderItems.ForEach(item => orderDetailsResponse.Products.Add(new OrderDetailsProductResponseDto
            {
                ProductId = item.ProductId,
                ImageUrl = item.Product.ImageUrl,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                Title = item.Product.Title,
                TotalPrice = item.TotalPrice,
                UnitPrice = prices.First(x => x.ProductId == item.ProductId && x.ProductTypeId == item.ProductTypeId).OriginalPrice
            }));


            response.Data = orderDetailsResponse;

            return response;
        }

        public async Task<ServiceResponse<List<OrderOverviewResponseDto>>> GetOrders()
        {
            try
            {
                var response = new ServiceResponse<List<OrderOverviewResponseDto>>();
                var orders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.UserId == _authService.GetUserId())
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                var orderResponse = new List<OrderOverviewResponseDto>();
                orders.ForEach(o => orderResponse.Add(new OrderOverviewResponseDto
                {
                    Id = o.Id,
                    //OrderNumber = $"RM{o.Id.ToString().PadLeft(4, '0')}",
                    OrderDate = o.OrderDate,
                    TotalPrice = o.TotalPrice,
                    Product = o.OrderItems.Count > 1 ? $"{o.OrderItems.First().Product.Title} and {o.OrderItems.Count - 1} more..."
                            : o.OrderItems.First().Product.Title,
                    ProductImageUrl = o.OrderItems.First().Product.ImageUrl
                }));

                response.Data = orderResponse;

                return response;
            }
            catch (Exception ex) 
            {
                return new ServiceResponse<List<OrderOverviewResponseDto>>(); ;
            }
        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            int userId= _authService.GetUserId();
            var products = (await _cartService.GetDbCartProducts(userId)).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));

            var order = new Order
            {
                UserId = userId,
                OrderDate = System.DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);

            _context.CartItems.RemoveRange(_context.CartItems.Where(ci => ci.UserId == userId));

            await _context.SaveChangesAsync();

            string orderNumber = $"RM{order.Id.ToString().PadLeft(4, '0')}";

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
