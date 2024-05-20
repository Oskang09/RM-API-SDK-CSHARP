
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RM_API_SDK_CSHARP;
using RM_API_SDK_CSHARP.Extension;
using RM_API_SDK_CSHARP.Model;
using RM_API_SDK_CSHARP.Model.Transaction;

namespace Api_Testing
{
    class Program
    {

        public static JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new JSONContractResolver(),
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
            Converters = new List<JsonConverter>()
            {
                new Newtonsoft.Json.Converters.StringEnumConverter(new DefaultNamingStrategy(), false),
            }
        };

        public static async Task Main()
        {
            RevenueMonsterOpenAPI rmApi = new RevenueMonsterOpenAPI(
                EnvironmentTarget.SANDBOX,
                "1715928248256575868",
                "yiTRttqXGoZnfIuLAWtPSjAVmcGwjqYv",
                @"-----BEGIN RSA PRIVATE KEY-----
MIIEoQIBAAKCAQB/Nozd2XVqI8FfQxdc9IPMG183Tt97qwAbpJPwGoAdLZNeqNQu
tn685sAzx8b3EyIZq/nj4lpduQeSi4LY5orNNbuc5IIfuuEiKN2I3MXRi17vKcwc
PN9IIdhfWPbIuLrUoFtTf4joQyuNu9bPNEtc8/zKU4cD3rzFjZAZXKoyTw57VCto
RKnt2X4+ns75FMhg6GqMUPHg3HV7RS3UNTxsHHGd9/hTd64hWBZ5wuY04zvvY2Z/
dlGmr3hW6/GjZg+eovpR7WfN14i46DhpM3x3j1zlzWowREOCv4yJIyGmNBAb1Yn3
5kUiPFdxtnFgT+gWJvku9wCHo7785v2blocVAgMBAAECggEAE4ALK96vrut4MuAX
7rnUki8H83R1sYFO7O3Xd5C4XsCjvHjovZhf6Rj+EXHoP+uA6KqCDbqe24sHbCFS
l2kPkdFQxwYIJFEDPbdB7j7ZW/ufO84Oaa2zFF2Ly2HZwn7+tKU4Gp6MH/5b6MgI
b462lrMX5vPS2HcGPMoscpbPyBI4f28lchTdzC/7hHJHWhpC6tAkfAgIWF1a8iuT
3rZ70YbQW7krmQR3YGL224JgQXRCrbtEDB41X6UaqnIBp/gqYwSUscA9rB4MvVTa
m+rNC69GNwjCwjfMP9KyBUgxDEwlrzs6aQYJ2EkTjWUyGe1rt2BLjZGJdCkVfnIk
71ZUKQKBgQDy3RWmoFnKsp+Vb3R4vR7GNrIuc/aqr8bhQEHxfcs94Lg7qFZ2kKVX
IMM1e7cAUV+m4NNy2PskQIufg703cfacOwp9/8GzR21iLMbP7vZYqIW3E71UgFxr
CskF9DXBeZGEQIN/CyG0AjOIbcpeA0GlM68yR6tCkNr5ykDDYi2WWwKBgQCGGA+b
ohfToqQUBGDtFSKTzA0spev6oZ+2l+jVonIVlKasPKNO13YH++JQmEmsm5aj1kw5
YwwV20w0Kqxa1lBAMIDlWkAbKRqSTelzDwW3PV62OT5DyTiqQKQrWp4DjraThHOw
Z1QdX8TTIarAKLnsHva1X/RcbHM1O2yz/oMzTwKBgBaH7DpJJgtoBuYWMPgCk5hP
+rfUik3LgOfkzjlZMkHLV0wxkdPuF2i94V5U2+VrpzQUJ4nAF4yFcG6cCjHaoMZd
s9t8nrvDJKK3BLrFsJUJDV6utiHgxrInjf6/aT1JyfY7BfgDUF0i1yURCnUuWCi7
YsWHYJwMVERYPynhMdmBAoGAMVsF06jz0Cfa8Ksz6c+PrIq8hqSDmPqISLLdNlTW
XO8b8eVXzpsrjIC/qh6Db47NAXzNDYpRVnIjOEhErCKZ4yZ3KvPvCJA2gYc/1+O9
nNZv1+515olX3AJy6qot4u1ScjWcR1+g7DblwThxGs1isXAsKlIpJSOdvAglWJLj
Q60CgYBJzpQ24237xErfI3jlMCgfjyE08c6ivqV1E7aC90X254qiUynVkYmHI2/p
v8p3jTdD1FpgAGqAVxoaSQ5ipgc4zJ4b1b66VgTuh5Z3IM9ta2GiPSRdFcKWOl84
SJftHXsoLv0iafOahZVXMzDMna46XD8zV7lXAyFCxsBGzpgWiw==
-----END RSA PRIVATE KEY-----",
                @"-----BEGIN RSA PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqCHYNviYOCuG4a1gWyRb
5BZrYqZBI0yrbLCQFtfS0zeJ1rXTfTZ/kY7e4mhFo14SmSWaU6DaMV2DW4W2Jy5r
BH/w4RyDynoAK54yK9Jhzg4ox64QDSjLWPv2sD8cZh4pryEBANmz6sfpfrjZJCAk
43sNe6NoqafL3V5kfMS9oJanSNVZAEfwJ/3LUHsSeIK5plJjUAbkjknQfJ1QPfIV
QaOwFlv1d8ChhPMtfSrBHuZzZuHgZMT6T/BjW6nfqLhbVoaKwxqiKMTBMh/f6c4y
6UGRD2xls6Hk7L33AxVDKQqtEPqjwmW80xS+0FkFGUMmB4elIo90Nt9zB1uuEMjX
0wIDAQAB
-----END RSA PUBLIC KEY-----"
            );

            rmApi.Initialize();
            var response = await rmApi.EnsureResponse(rmApi.OAuth.Authenticate());
            rmApi.OAuth.UpdateToken(response.AccessToken, response.RefreshToken, response.TokenType);

            rmApi.SetupOpenApiSdk();

            // var transaction = await rmApi.EnsureResponse(rmApi.Payment.QueryStatusByTransactionID("211208030149010416535734"));


            try
            {
                var json = new
                {
                    transactionId = "240516083021310428287897",
                    refund = new
                    {
                        type = "FULL",
                        currencyType = "MYR",
                        amount = 1850
                    },
                    reason = "oska test",
                };
                HttpClient client = rmApi.UseOpenApiHttpClient();
                var dm = await rmApi.EnsureResponse<Transaction>(client, HttpMethod.Post, "/v3/payment/refund", json);
                Console.WriteLine(dm);
                // EnsureResponse will automatically process the error & response for you
                // var selfResponse = await rmApi.EnsureResponse<Transaction>(client.GetAsync("/v3/payment/transaction/2112080301490104165351734"));
                // Console.WriteLine(selfResponse.TransactionId);
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