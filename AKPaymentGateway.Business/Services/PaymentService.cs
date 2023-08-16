using AKPaymentGateway.Data;
using AKPaymentGateway.Models;

namespace AKPaymentGateway.Business;

public class PaymentService : IPaymentService
{
    public PaymentDetailsResponse ProcessPayment(PaymentRequest request)
    {
        // Simulate interaction with the bank simulator.
        // In a real implementation, you would communicate with the actual bank's API.

        // For the sake of this example, let's assume that payment is successful if the CVV is "123".
        bool isPaymentSuccessful = request.CVV == "123";

        string paymentId = Guid.NewGuid().ToString();

        if (isPaymentSuccessful)
        {
            var paymentDetails = new PaymentDetails
            {
                PaymentId = paymentId,
                CardNumber = request.CardNumber,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                // Other payment details...
            };

            PaymentDatabase.Payments.Add(paymentDetails);

            return new PaymentDetailsResponse
            {
                PaymentId = paymentId,
                Status = "Success",
                Message = "Payment processed successfully."
            };
        }
        else
        {
            return new PaymentDetailsResponse
            {
                PaymentId = paymentId,
                Status = "Failure",
                Message = "Payment processing failed."
            };
        }
    }

    public PaymentDetailsResponse GetPaymentDetails(string paymentId)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
            return null;

        var paymentDetails = PaymentDatabase.Payments.FirstOrDefault(p => p.PaymentId == paymentId);


        if (paymentDetails == null)
        {
            // Payment not found.
            return new PaymentDetailsResponse
            {
                Status = "Failure",
                Message = "No Payment detail found."
            };
        }

        return new PaymentDetailsResponse
        {
            PaymentId = paymentDetails.PaymentId,
            MaskedCardNumber = MaskCardNumber(paymentDetails.CardNumber),
            Status = "Success",
        };
    }

    private string MaskCardNumber(string cardNumber)
    {
        // Logic to mask the card number, e.g., "**** **** **** 1234".
        // Implement as needed.
        return "**** **** **** " + cardNumber.Substring(cardNumber.Length - 4);
    }


}