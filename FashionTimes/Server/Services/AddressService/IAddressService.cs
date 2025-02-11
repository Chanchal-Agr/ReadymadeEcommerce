﻿using FashionTimes.Shared.Entities;

namespace FashionTimes.Server.Services.AddressService
{
    public interface IAddressService
    {
        Task<ServiceResponse<Address>> GetAddress();
        Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address);
    }
}
