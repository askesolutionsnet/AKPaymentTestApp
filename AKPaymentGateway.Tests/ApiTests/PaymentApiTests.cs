using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AKPaymentGateway.Business;
using AKPaymentGateway.Models;
using AKPaymentGateway.Data;
using AKPaymentGateway.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AKPaymentGateway.ApiTests.Tests;

[TestClass]
public class PaymentApiTests
{
    private PaymentController _paymentController;
    private Mock<IPaymentService> _mockPaymentService;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockPaymentService = new Mock<IPaymentService>();
        _paymentController = new PaymentController(_mockPaymentService.Object);
    }

    [TestMethod]
    public void ProcessPayment_ValidRequest_ReturnsOk()
    {
        // Arrange
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "1111111111111111",
            ExpiryMonth = "05",
            ExpiryYear = "2025",
            CVV = "123",
            Amount = 100,
            Currency = "GBP"
        };

        var paymentResponse = new PaymentDetailsResponse { Status = "Success" };
        _mockPaymentService.Setup(service => service.ProcessPayment(paymentRequest)).Returns(paymentResponse);

        // Act
        var result = _paymentController.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ActionResult<PaymentDetailsResponse>));

        var okResult = result as ActionResult<PaymentDetailsResponse>;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Result, typeof(OkObjectResult));

        var okObjectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(okObjectResult);
        Assert.AreEqual(StatusCodes.Status200OK, okObjectResult.StatusCode);

        var responseValue = okObjectResult.Value as PaymentDetailsResponse;
        Assert.IsNotNull(responseValue);
        Assert.AreEqual(paymentResponse.Status, responseValue.Status);
    }

    [TestMethod]
    public void ProcessPayment_PaymentServiceReturnsFailure_ReturnsBadRequest()
    {
        // Arrange
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "1111111111111111",
            ExpiryMonth = "05",
            ExpiryYear = "2025",
            CVV = "123",
            Amount = 100,
            Currency = "GBP"
        };

        var paymentResponse = new PaymentDetailsResponse { Status = "Failure", Message = "Payment processing failed." };
        _mockPaymentService.Setup(service => service.ProcessPayment(paymentRequest)).Returns(paymentResponse);

        // Act
        var result = _paymentController.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [TestMethod]
    public void ProcessPayment_ExceptionThrown_ReturnsBadRequest()
    {
        // Arrange
        var paymentRequest = new PaymentRequest { /* fill with valid data */ };
        _mockPaymentService.Setup(service => service.ProcessPayment(paymentRequest)).Throws(new Exception("Something went wrong."));

        // Act
        var result = _paymentController.ProcessPayment(paymentRequest);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [TestMethod]
    public void GetPaymentDetails_ValidPaymentId_ReturnsOk()
    {
        // Arrange
        var paymentId = "valid-payment-id";
        var paymentResponse = new PaymentDetailsResponse { Status = "Success" };
        _mockPaymentService.Setup(service => service.GetPaymentDetails(paymentId)).Returns(paymentResponse);

        // Act
        var result = _paymentController.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(ActionResult<PaymentDetailsResponse>));

        var okResult = result as ActionResult<PaymentDetailsResponse>;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Result, typeof(OkObjectResult));

        var okObjectResult = okResult.Result as OkObjectResult;
        Assert.IsNotNull(okObjectResult);
        Assert.AreEqual(StatusCodes.Status200OK, okObjectResult.StatusCode);

        var responseValue = okObjectResult.Value as PaymentDetailsResponse;
        Assert.IsNotNull(responseValue);
        Assert.AreEqual(paymentResponse.Status, responseValue.Status);
    }

    [TestMethod]
    public void GetPaymentDetails_InvalidPaymentId_ReturnsNotFound()
    {
        // Arrange
        var paymentId = "invalid-payment-id";
        var paymentResponse = new PaymentDetailsResponse { Status = "Failure", Message = "No Payment detail found." };
        _mockPaymentService.Setup(service => service.GetPaymentDetails(paymentId)).Returns(paymentResponse);

        // Act
        var result = _paymentController.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [TestMethod]
    public void GetPaymentDetails_ExceptionThrown_ReturnsBadRequest()
    {
        // Arrange
        var paymentId = "valid-payment-id";
        _mockPaymentService.Setup(service => service.GetPaymentDetails(paymentId)).Throws(new Exception("Something went wrong."));

        // Act
        var result = _paymentController.GetPaymentDetails(paymentId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

}