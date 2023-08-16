# AKPaymentGateway


# Technology: 
1- Used .Net 7 framework in Visual Studio 2022 
2- Used Restful API to call from UI and integrated with Swagger UI to test API 
3- Console App as simulator
4- Used MS Unit Test

# Instructions: 
1- Cloned the project in the local make sure you have .Net 7 framework installed in Visual Studio 2022. 
2- Run solution with multiple project with API and Console App Simulator project .
3- Can also test Payment API using Swagger UI.
4- Used In-memory object to retain the data
5- Can run the unit test to verfiy the code

# Understanding the Program Structure:

The program defines a payment gateway system, including models, business logic, data management, and an API for processing and retrieving payment details.
# Models:

The PaymentRequest class represents the information required to process a payment request.
The PaymentDetailsResponse class represents the response after processing a payment request.
The PaymentDetails class represents detailed payment information.
The PaymentDatabase class maintains a list of payment details.

# Business Logic:

The IPaymentService interface defines methods for processing payments and retrieving payment details.
The PaymentService class implements the IPaymentService interface and contains logic to simulate payment processing and retrieval of payment details.

# API Controllers:

The PaymentController class contains endpoints to interact with the payment gateway.
The ProcessPayment method accepts a PaymentRequest, simulates payment processing, and returns a response.
The GetPaymentDetails method retrieves payment details based on a payment ID.

# Dependency Injection and Configuration:

Dependency injection is used to inject the PaymentService into the PaymentController.
JSON serialization options, API explorer, Swagger, and CORS policies are configured in the service collection.
Usage Instructions:

Open a terminal or command prompt.
Navigate to the project directory.
Use the dotnet run command to start the application.
The application starts a web server and listens for incoming requests.
API Endpoints:

# Process Payment:

Endpoint: POST /api/payments/process
Request: Send a PaymentRequest object containing card details and payment information.
Response: Returns a PaymentDetailsResponse indicating success or failure.
Get Payment Details:

Endpoint: GET /api/payments/details/{paymentId}
Request: Provide a valid paymentId in the URL.
Response: Returns detailed payment information using a PaymentDetailsResponse.
Testing the API:

Use tools like curl, Postman, or any API testing tool to send requests to the API endpoints.
For example, to process a payment, send a POST request with a PaymentRequest JSON object to /api/payments/process.
Swagger UI (Development Only):

If in development mode, access the Swagger UI at /swagger to explore and interact with the API endpoints.

# Notes:

The provided code includes basic simulation logic for payment processing and masking card numbers for demonstration purposes. In a real-world scenario, you would integrate with actual payment gateways and banks.
The program lacks security measures such as authentication and authorization. In a production environment, ensure proper security mechanisms are implemented.
