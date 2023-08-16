namespace AKPaymentGateway.Models;

public class PaymentDetails
{
    public string PaymentId { get; set; }
    public string CardNumber { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; }
}
