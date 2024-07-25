using FashionTimes.Shared.Entities;
using Stripe.Checkout;

namespace FashionTimes.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
        Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
    }
}
