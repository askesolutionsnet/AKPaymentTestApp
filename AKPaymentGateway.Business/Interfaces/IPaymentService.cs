using AKPaymentGateway.Models;

namespace AKPaymentGateway.Business
{
    public interface IPaymentService
    {
        PaymentDetailsResponse ProcessPayment(PaymentRequest request);
        PaymentDetailsResponse GetPaymentDetails(string paymentId);
    }
}
