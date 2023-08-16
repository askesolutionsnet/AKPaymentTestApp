using System.Text.Json;
using System.Text;
using AKPaymentGateway.Models;
class Program 
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:5266"); // Replace with your API base URL

        Console.WriteLine("Bank Simulator - Payment Gateway Console App");
        Console.WriteLine("------------------------------------------");

        var paymentRequest = new PaymentRequest
        {
            CardNumber = GetUserInput("Card Number: "),
            ExpiryMonth = GetUserInput("Expiry Month: "),
            ExpiryYear = GetUserInput("Expiry Year: "),
            CVV = GetUserInput("CVV: "),
            Amount = Convert.ToDecimal(GetUserInput("Amount: ")),
            Currency = GetUserInput("Currency: ")
        };

        Console.WriteLine("\nChoose an option:");
        Console.WriteLine("1. Process Payment");
        Console.WriteLine("2. Get Payment Details by ID");
        Console.Write("Enter your choice (1/2): ");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await ProcessPayment(httpClient, paymentRequest);
                break;
            case "2":
                await GetPaymentDetailsById(httpClient);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }

        Console.ReadLine();
    }

    static async Task ProcessPayment(HttpClient httpClient, PaymentRequest paymentRequest)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await httpClient.PostAsync("/api/payments/process", jsonContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PaymentDetailsResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                PropertyNameCaseInsensitive = true
            });

            Console.WriteLine($"Payment ID: {paymentResponse.PaymentId}");
            Console.WriteLine($"Status: {paymentResponse.Status}");
            Console.WriteLine($"Message: {paymentResponse.Message}");
        }
        else
        {
            Console.WriteLine("Payment processing failed.");
        }
    }

    static async Task GetPaymentDetailsById(HttpClient httpClient)
    {
        Console.Write("Enter Payment ID: ");
        string paymentId = Console.ReadLine();

        HttpResponseMessage response = await httpClient.GetAsync($"/api/payments/{paymentId}");

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonSerializer.Deserialize<PaymentDetailsResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                PropertyNameCaseInsensitive = true
            });

            Console.WriteLine($"Payment ID: {paymentResponse.PaymentId}");
            Console.WriteLine($"Masked Card Number: {paymentResponse.MaskedCardNumber}");
            Console.WriteLine($"Status: {paymentResponse.Status}");
        }
        else
        {
            Console.WriteLine("Payment details retrieval failed.");
        }
    }

    static string GetUserInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }
}