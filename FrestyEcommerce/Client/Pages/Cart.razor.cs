using FrestyEcommerce.Shared.Dtos;
using Microsoft.AspNetCore.Components;
using System;

namespace FrestyEcommerce.Client.Pages
{
    public partial class Cart
    {
        List<CartProductResponseDto> cartProducts = null;
        string message = "Loading cart...";
        bool isAuthenticated = false;

        protected override async Task OnInitializedAsync()
        {
            isAuthenticated = await AuthService.IsUserAuthenticated();
            if (isAuthenticated)

                await LoadCart();
        }

        private async Task RemovProductFromCart(int productId, int prodctTypeId)
        {
            await CartService.RemoveProductFromCart(productId, prodctTypeId);
            await LoadCart();
        }

        private async Task LoadCart()
        {
            await CartService.GetCartItemsCount();
            cartProducts = await CartService.GetCartProducts();

            if (cartProducts == null || cartProducts.Count == 0)
            {
                message = "Your cart is empty.";
                cartProducts = new List<CartProductResponseDto>();
            }
        }

        private async Task UpdateQuantity(ChangeEventArgs e, CartProductResponseDto product)
        {
            product.Quantity = int.Parse(e.Value.ToString());
            if (product.Quantity < 1)
            {
                product.Quantity = 1;
            }
            await CartService.UpdateQuantity(product);
        }

        private async Task PlaceOrder()
        {
            bool value = await OrderService.PlaceOrder();
            if (value)
                NavigationManager.NavigateTo("/order-success");
            else
                NavigationManager.NavigateTo("/login");
        }
    }
}