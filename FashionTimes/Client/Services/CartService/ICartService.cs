using FashionTimes.Shared.Dtos;
using FashionTimes.Shared.Entities;

namespace FashionTimes.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartProductResponseDto>> GetCartProducts();
        Task RemoveProductFromCart(int productId, int productTypeId);
        Task UpdateQuantity(CartProductResponseDto product);
        Task StoreCartItems(bool emptyLocalCart);
        Task GetCartItemsCount();

    }
}
