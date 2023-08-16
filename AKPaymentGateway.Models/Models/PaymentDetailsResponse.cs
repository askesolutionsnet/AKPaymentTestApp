namespace AKPaymentGateway.Models;

public class PaymentDetailsResponse
{
    public string PaymentId { get; set; }
    public string MaskedCardNumber { get; set; }
    public string Status { get; set; }  
    public string Message { get; set; }
}