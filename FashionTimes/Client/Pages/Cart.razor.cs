using FashionTimes.Shared.Dtos;
using FashionTimes.Shared.Entities;
using Microsoft.AspNetCore.Components;
using System;

namespace FashionTimes.Client.Pages
{
    public partial class Cart
    {
        List<CartProductResponseDto> cartProducts = null;
        string message = "Loading cart...";
        bool isAuthenticated = false;
        Address? Address { get; set; } = null;


        protected override async Task OnInitializedAsync()
        {
            isAuthenticated = await AuthService.IsUserAuthenticated();
            if (isAuthenticated)
            {
                await LoadCart();
                Address = await AddressService.GetAddress();
            }
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
            if (Address != null)
            {
                bool value = await OrderService.PlaceOrder();
                if (value)
                    NavigationManager.NavigateTo("/order-success");
                else
                    NavigationManager.NavigateTo("/login");
            }
            else
            {
                message = "Please add your address first.";
            }
        }

        private async Task OnAddressAdded(bool val)
        {
            Address = await AddressService.GetAddress();
        }
    }
}