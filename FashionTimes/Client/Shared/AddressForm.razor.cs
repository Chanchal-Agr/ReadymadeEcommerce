using FashionTimes.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace FashionTimes.Client.Shared
{
    public partial class AddressForm
    {
        Address address = null;
        bool editAddress = false;

        [Parameter]
        public EventCallback<bool> OnConfirm { get; set; }
        private async Task Confirm()
        {
            await OnConfirm.InvokeAsync(true);
        }


        protected override async Task OnInitializedAsync()
        {
            address = await AddressService.GetAddress();
        }

        private async Task SubmitAddress()
        {
            editAddress = false;
            address = await AddressService.AddOrUpdateAddress(address);
            await Confirm();
        }

        private void InitAddress()
        {
            address = new Address();
            editAddress = true;
        }

        private void EditAddress()
        {
            editAddress = true;
        }
    }
}