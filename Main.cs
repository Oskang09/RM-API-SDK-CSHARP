
using System;
using System.Net.Http;
using System.Threading.Tasks;
using RM_API_SDK_CSHARP.Model;
using RM_API_SDK_CSHARP.Model.Transaction;

namespace RM_API_SDK_CSHARP
{
    class Program
    {
        public static async Task Main()
        {
            RevenueMonsterOpenAPI rmApi = new RevenueMonsterOpenAPI(
                Model.EnvironmentTarget.PRODUCTION,
                "1638207549250413318",
                "ruBvyGtvioQUQSRZJePlrEalmNkNYsKX",
                @"-----BEGIN RSA PRIVATE KEY-----
MIIEpgIBAAKCAQEAvPh+trdfKIAFROi/fqNAtX0bmsBSFgdWHGQmRFu5drRzWCGw
LOAZ5vcw0N0pLhtMbE18ANCVoS2KwQB2lFWYOrKXJP6jAoqN05HRbecuKlpV/11j
nb405Md8ZCMYikfpLLDO0gFM0r67D8cclezsIqD5qyoY3z8PMbPonjV08zfXdAuW
TCopuqMrsLXI+/sYP0Qe4j6iY/2xQYiCSyWU3puqqT5s/wEy8xA9ulQlDtPcWJph
+7Hp2I9t37q1v7fkSM5Cgzrs28CCFBeI+/Fu/+RGtYu8zemmH2UTfpVd6VpYtFrE
oxsJ+JdwvJeOEiRWeZ3ZcwWwUDhGj5xrqcp3sQIDAQABAoIBAQC6+bD+/y8zdoBA
L38SQVvMd25xzyspSrcEPn+ykYNPlbqvEB4uOMrIQftWHg4Z5b7XRk+Uys3SfapV
zyyFFrAaHAz6+My3vfoYxaYP9XczRtDibDgdo07Ysx08Q5GLeR2ZL5RkLA8kUr3q
HZwGKDd5CAghOPUFJ3LUPevDZqdB4l58LCmqbjj+mOQArYWH84ZK3qZvJJrCtp+S
826gfhqHcnTDknbcD3wqzrSrECFKiKpugLTuyLZyyNX859eKVXLWP59hliRbAhpG
fOTm0yAwfyHdUs/3WD7OuhWZOXeABs58s0nLfv3reb5bMkCRv1rm8cfIyfTBtGyM
TJ3c884BAoGBAPxHoy/xp5A95k4bvSc4ltWuM/eFbhUc6YkKtaP3w8xzgfqoW4YX
8oZuzUcpokInKgKv04UjexFPUQgOAjC/z8PFoEL595gCeWBzplOygPZFMyY0CjMK
NXU521WAWhIpcDQA8C6+rs2cl0/gw+ItXvmYOTTBJyP08dnnd8YdMWXRAoGBAL/B
3Sqx4o4RF2toOFBybaq2yQgVmEZ4o4v3gPKgQIqr4Y9Kj4lzSNVF+vrKVKjpdsDF
bhC5wwCd7vWgJN8EthtTe8vmrfW4Ws5zV0z3o6XZ3PFp8D9NjxSTHmHhShwCj7sk
/V6ZY+t/RKkCYyVlvIttvMfePZNPYiU6nttCAAvhAoGBAPW0e85VKOLGNsk6IPbT
GazRtpFLZhSwDQ/I23Hv/MXideSAFmu6ZR3jSpEVjo+pfzQQtQ0FqnYIbx5OLnn2
Rs8gal7w/+EP/9Edsg/wWcQBT7jUlF5zge9DQ0i58n+f1kT/6O1OEAf3jDNi8MUX
dVnYlVa5DHrrtKWK18+fXR4RAoGBAJvUZXDuwheFWCzWsvzdILXceJ3zPp32J6H0
7ns9M3X9m/2gL+kZIJUAm6b2iboCj5i0WG+LJib4MVq2W4BJCdxHCZF30uKegFzJ
aqaHkBuVYnFlD/HBi4bCz6vnMxx0EG2a77PSn1qfQCLAZ+XaOVBtSz55Ijz2A+IR
M0sm5XbBAoGBAPs4aE2+5QT9CviAqiuDpRevzKbzGgFxr+O2qE8gf0XnGRvRLzWc
+E7O8LhhtefQTJecSIt2jHE9f/WUg8YmXPEZd2mQeBfTD7lehX6ASZiod+OmvoYs
QDkmneews92u9YJTQjvgFRwpLwmKYndwwSDIbliZ1dytrMG4fDPRkoKx
-----END RSA PRIVATE KEY-----",
                @"-----BEGIN RSA PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAtwT9XrgJRajwu6zzutbJ
HicOgvcViPAlT7h8BNc9IJ9UdeUfQJyX3AWQcPsfeAr/EfkEnALw0KkOFNLVbaQH
7Ld06PCS5kJ8ubTsqvsDilxTFWIU2OIKD3C9LJ7WMVSE2DALvriBL9dRpEozeRXM
+hS1pXS9qQP/ee+3uKBbErq1g2ICQukvqlZw7DUk2y/UC5FjHWc9KWH8m8fe5H4e
wL57BmuyJncoFf+w6R/jGG9vhTZAbADCP3xSnpoOn5cdRXEW2VpiPs7wBhECw+Ni
asLF4EXoF9A8EofH8Ms/tlrDXNbDMkq//lnLL3A0wdw/HvuvE0NvL6iwuYKURDXM
BQIDAQAB
-----END RSA PUBLIC KEY-----"
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