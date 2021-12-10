
using System;
using System.Net.Http;
using System.Threading.Tasks;
using RM_API_SDK_CSHARP;
using RM_API_SDK_CSHARP.Model;
using RM_API_SDK_CSHARP.Model.Transaction;

namespace Api_Testing
{
    class Program
    {
        public static async Task Main()
        {
            RevenueMonsterOpenAPI rmApi = new RevenueMonsterOpenAPI(
                EnvironmentTarget.PRODUCTION,
                "1638207549250413318",
                "ruBvyGtvioQUQSRZJePlrEalmNkNYsKX",
                @"private-key",
                @"public-key"
            );

            rmApi.Initialize();
            var response = await rmApi.EnsureResponse(rmApi.OAuth.Authenticate());
            rmApi.OAuth.UpdateToken(response.AccessToken, response.RefreshToken, response.TokenType);

            rmApi.SetupOpenApiSdk();

            var transaction = await rmApi.EnsureResponse(rmApi.Payment.QueryStatusByTransactionID("211208030149010416535734"));

            HttpClient client = rmApi.UseOpenApiHttpClient();

            try
            {
                // EnsureResponse will automatically process the error & response for you
                var selfResponse = await rmApi.EnsureResponse<Transaction>(client.GetAsync("/v3/payment/transaction/2112080301490104165351734"));
                Console.WriteLine(selfResponse.TransactionId);
            }
            catch (ApiErrorException error)
            {
                // Error Handling
                Console.WriteLine(error.Code);
                Console.WriteLine(error.Message);
            }

            // var quickpayRequest = new Model.Request.QuickPayRequest
            // {
            //     Order = new Model.Request.QuickPayRequestOrder
            //     {
            //         Id = "123123123a1a",
            //         Title = "RM Debug",
            //         Detail = "RM Detail",
            //         CurrencyType = "MYR",
            //         Amount = 10,
            //     },
            //     AuthCode = "884239702898753798979433",
            //     IpAddress = "0.0.0.0",
            //     StoreId = "1602660043994159611",
            // };
            // var quickPayResponse = await rmApi.EnsureResponse(rmApi.Payment.QuickPay(quickpayRequest));

            // Console.WriteLine(JsonConvert.SerializeObject(quickPayResponse));
        }
    }
}