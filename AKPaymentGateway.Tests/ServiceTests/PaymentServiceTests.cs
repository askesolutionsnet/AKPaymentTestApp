using Microsoft.VisualStudio.TestTools.UnitTesting;
using AKPaymentGateway.Business;
using AKPaymentGateway.Models;
using AKPaymentGateway.Data;

namespace AKPaymentGateway.ServiceTests.Tests;

[TestClass]
public class PaymentApiTests
{
    private PaymentService _paymentService;

    [TestInitialize]
    public void TestInitialize()
    {
        _paymentService = new PaymentService();
    }

    [TestMethod]
    public void ProcessPayment_SuccessfulPayment_ReturnsSuccessResponse()
    {
        // Arrange
        var paymentRequest = new PaymentRequest { CVV = "123", CardNumber = "1111111111111111" };

        // Act
        var result = _paymentService.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Success", result.Status);
    }

    [TestMethod]
    public void ProcessPayment_FailedPayment_ReturnsFailureResponse()
    {
        // Arrange
        var paymentRequest = new PaymentRequest { CVV = "456", CardNumber = "1111111111111111" };

        // Act
        var result = _paymentService.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Failure", result.Status);
    }

    [TestMethod]
    public void GetPaymentDetails_ValidPaymentId_ReturnsPaymentDetails()
    {
        // Arrange
        var paymentId = "valid-payment-id";
        PaymentDatabase.Payments.Add(new PaymentDetails { PaymentId = paymentId, CardNumber = "1111111111111111" });

        // Act
        var result = _paymentService.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Success", result.Status);
    }

    [TestMethod]
    public void GetPaymentDetails_InvalidPaymentId_ReturnsNotFoundResponse()
    {
        // Arrange
        var paymentId = "invalid-payment-id";

        // Act
        var result = _paymentService.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Failure", result.Status);
        Assert.AreEqual("No Payment detail found.", result.Message);
    }

    [TestMethod]
    public void GetPaymentDetails_MaskedCardNumber_ReturnsMaskedNumber()
    {
        // Arrange
        var paymentId = "masked-payment-id";
        var cardNumber = "1234567890123456";
        PaymentDatabase.Payments.Add(new PaymentDetails { PaymentId = paymentId, CardNumber = cardNumber });

        // Act
        var result = _paymentService.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Success", result.Status);
        Assert.AreEqual("**** **** **** 3456", result.MaskedCardNumber);
    }

    [TestMethod]
    public void ProcessPayment_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var paymentRequest = new PaymentRequest();

        // Act
        var result = _paymentService.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Failure", result.Status);
        Assert.AreEqual("Payment processing failed.", result.Message);
    }

    [TestMethod]
    public void GetPaymentDetails_MissingPaymentId_ReturnsNull()
    {
        // Act
        var result = _paymentService.GetPaymentDetails(null);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetPaymentDetails_ValidPaymentId_MissingPaymentDetails_ReturnsNotFound()
    {
        // Arrange
        var paymentId = "valid-payment-id";

        // Act
        var result = _paymentService.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Failure", result.Status);
        Assert.AreEqual("No Payment detail found.", result.Message);
    }

    [TestMethod]
    public void ProcessPayment_NegativeAmount_ReturnsBadRequest()
    {
        // Arrange
        var paymentRequest = new PaymentRequest { Amount = -100 };

        // Act
        var result = _paymentService.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Failure", result.Status);
        Assert.AreEqual("Payment processing failed.", result.Message);
    }

    [TestMethod]
    public void ProcessPayment_CVVIsNull_ReturnsBadRequest()
    {
        // Arrange
        var paymentRequest = new PaymentRequest { CVV = null, CardNumber = "1111111111111111" };

        // Act
        var result = _paymentService.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Failure", result.Status);
        Assert.AreEqual("Payment processing failed.", result.Message);
    }

}