# RM-API-SDK-CSHARP

### Installation

Download latest from `https://github.com/Oskang09/RM-API-SDK-CSHARP/releases`. There's dll version & nuget.

### Covered Functions

- [x] Client Credentials (Authentication)
- [x] Refresh Token (Authentication)
- [x] Payment - QuickPay
- [x] Payment - Query Status By Order ID
- [x] Payment - Query Status By Transaction ID
- [ ] Payment (Transaction QR) - Create Transaction QR
- [ ] Payment ( Web / Mobile ) - Create Transaction
- [ ] Payment ( Web / Mobile ) - Notify Response Transformer
- [ ] Payment ( Web / Mobile ) - Get QRCode & URL By Checkout ID
- [ ] Payment ( Web / Mobile ) - Get Online Transaction By Checkout ID
- [ ] Payment - Refund
- [ ] Payment - Reverse


## Authenticating

### Manual 

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM_API_SDK_CSHARP
{
    class Program
    {
        public static async Task Main()
        {
            RevenueMonsterOpenAPI openapi = new RevenueMonsterOpenAPI(
                Model.EnvironmentTarget.PRODUCTION,
                "client_id",
                "client_secret",
                @"your_private_key",
                @"server_public_key"
            );

            // Intialize configuration and pre verification stuff
            openapi.Initialize();

            // Manually handle token authentication
            var response = await openapi.EnsureResponse(openapi.OAuth.Authenticate());
            openapi.OAuth.UpdateToken(response.AccessToken, response.RefreshToken, response.TokenType);

            // Will automatically use the token, you updated using `UpdateToken`
            response = await openapi.EnsureResponse(openapi.OAuth.AuthenticateWithRefreshToken());
            openapi.OAuth.UpdateToken(response.AccessToken, response.RefreshToken, response.TokenType);
        }
    }
}
```

### Auto Authenticate & Refresh Handling 

```csharp

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM_API_SDK_CSHARP
{
    class Program
    {
        public static async Task Main()
        {
            RevenueMonsterOpenAPI openapi = new RevenueMonsterOpenAPI(
                Model.EnvironmentTarget.PRODUCTION,
                "client_id",
                "client_secret",
                @"your_private_key",
                @"server_public_key"
            );

            // Intialize configuration and pre verification stuff
            openapi.Initialize();

            // Auto refresh token every 30minutes by default, you can specify interval & onRefresh delegate handler
            var timer = openapi.OAuth.AutoAuthenticate("", "");

            // when you want to stop the auto refresh timer
            await timer.DisposeAsync();

        }
    }
}
```

# Open API Calling

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM_API_SDK_CSHARP
{
    class Program
    {
        public static async Task Main()
        {
            RevenueMonsterOpenAPI openapi = new RevenueMonsterOpenAPI(
                Model.EnvironmentTarget.PRODUCTION,
                "client_id",
                "client_secret",
                @"your_private_key",
                @"server_public_key"
            );

            openapi.Initialize();

            var response = await openapi.EnsureResponse(openapi.OAuth.Authenticate());
            openapi.OAuth.UpdateToken(response.AccessToken, response.RefreshToken, response.TokenType);

            openapi.SetupOpenApiSdk();

            try
            {
                // EnsureResponse will automatically process the error & response for you
                await openapi.EnsureResponse(rmApi.Payment.QueryStatusByTransactionID("transaction_id"));
            }
            catch (ApiErrorException error)
            {
                // Error Handling
                Console.WriteLine(error.Code);
                Console.WriteLine(error.Message);
                // TRANSACTION_NOT_FOUND
                // No payment transaction
            }

        }
    }
}
```

### Custom / Self Built API

If there's some new Feature / Open API, is not updated only we suggest to use this method, we handled most of the data processing & security check part. What you need to do only pass the request parameters and url.

```csharp
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RM_API_SDK_CSHARP
{
    class Program
    {
        public static async Task Main()
        {
            RevenueMonsterOpenAPI openapi = new RevenueMonsterOpenAPI(
                Model.EnvironmentTarget.PRODUCTION,
                "client_id",
                "client_secret",
                @"your_private_key",
                @"server_public_key"
            );

            openapi.Initialize();

            var response = await openapi.EnsureResponse(openapi.OAuth.Authenticate());
            openapi.OAuth.UpdateToken(response.AccessToken, response.RefreshToken, response.TokenType);

            HttpClient client = rmApi.UseOpenApiHttpClient();
            try
            {
                // EnsureResponse will automatically process the error & response for you
                // You will need handle the response yourself, you can pass it your own built class
                await rmApi.EnsureResponse<Transaction>(client, HttpMethod.Get, "/v3/payment/transaction/211208030149010416535734");
            }
            catch (ApiErrorException error)
            {
                // Error Handling
                Console.WriteLine(error.Code);
                Console.WriteLine(error.Message);
                // TRANSACTION_NOT_FOUND
                // No payment transaction
            }
        }
    }
    
    public class Transaction
    {
        public string ReferenceId { get; set; }
        public string TransactionId { get; set; }
        public string TerminalId { get; set; }
        public string CurrencyType { get; set; }
        public Int64 BalanceAmount { get; set; }
        public string Platform { get; set; }
        public string Method { get; set; }
        public DateTime TransactionAt { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
```