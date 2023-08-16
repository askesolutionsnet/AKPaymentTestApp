using AKPaymentGateway.Business;
using AKPaymentGateway.Data;
using AKPaymentGateway.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace AKPaymentGateway.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [Route("process")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PaymentDetailsResponse> ProcessPayment([FromBody] PaymentRequest request)
    {
        try
        {
            //Data validation check
            if (string.IsNullOrWhiteSpace(request.CardNumber) || string.IsNullOrWhiteSpace(request.ExpiryMonth)
                    || string.IsNullOrWhiteSpace(request.ExpiryYear) || string.IsNullOrWhiteSpace(request.Currency)
                    || string.IsNullOrWhiteSpace(request.CVV) || request.Amount <=0)
                return BadRequest("Invalid Payment Request");


            // Simulate payment processing logic and interaction with bank simulator.
            var response = _paymentService.ProcessPayment(request);

            // Return a un-successful response.
            if (response == null || response.Status.Equals("Failure"))
                return BadRequest("Payment processed un-successfull");

            // Return a successful response.
            return Ok(response);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("details/{paymentId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<PaymentDetailsResponse> GetPaymentDetails(string paymentId)
    {
        try
        {
            //Data validation check
            if (string.IsNullOrWhiteSpace(paymentId))
                return BadRequest("Payment Id required");

            var response = _paymentService.GetPaymentDetails(paymentId);

            // Return a un-successful response.
            if (response == null || response.Status.Equals("Failure"))
                return NotFound(response.Message);


            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}