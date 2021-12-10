using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RM_API_SDK_CSHARP.Model;
using RM_API_SDK_CSHARP.Util;

namespace RM_API_SDK_CSHARP.Extension
{
    public class OpenAPIInterceptor : DelegatingHandler
    {
        private RSACryptoServiceProvider signer;
        private RSACryptoServiceProvider verifier;
        private String signType;
        private RevenueMonsterOpenAPI rmOpenApi;

        public OpenAPIInterceptor(RevenueMonsterOpenAPI rmOpenApi, RSACryptoServiceProvider signer, RSACryptoServiceProvider verifier, String signType, HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
        {
            this.signer = signer;
            this.verifier = verifier;
            this.rmOpenApi = rmOpenApi;
            this.signType = signType;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            int nonceLength = 32;
            string url = request.RequestUri.OriginalString;
            string unix = DateTime.Now.UnixSecond().ToString();
            string nonce = nonceLength.RandomString();
            string method = request.Method.ToString();
            string body = "";
            if (request.Content != null)
            {
                body = await request.Content.ReadAsStringAsync();
            }

            string signature = signer.GenerateSignature(body, url, nonce, signType, method, unix);

            request.Headers.Add("Authorization", this.rmOpenApi.OAuth.TokenType + " " + this.rmOpenApi.OAuth.AccessToken);
            request.Headers.Add("X-Signature", signType + " " + signature);
            request.Headers.Add("X-Nonce-Str", nonce);
            request.Headers.Add("X-Timestamp", unix);

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            string respBody = "";
            if (response.Content != null)
            {
                var deserializedObject = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                sortJSONByKey(deserializedObject);

                respBody = JsonConvert.SerializeObject(deserializedObject);
            }

            if (!response.Headers.Contains("X-Signature"))
            {
                return response;
            }

            string respXSignature = response.Headers.GetValues("X-Signature").FirstOrDefault();
            string respNonce = response.Headers.GetValues("X-Nonce-Str").FirstOrDefault();
            string respUnix = response.Headers.GetValues("X-Timestamp").FirstOrDefault();
            string[] respSignArray = respXSignature.Split(" ");
            string respSignType = respSignArray[0];
            string respSignature = respSignArray[1];

            bool isValidSignature = verifier.VerifySignature(respSignature, respBody, url, respNonce, respSignType, method, respUnix);
            if (!isValidSignature)
            {
                throw new ApiErrorException("fail to verify signature using provided public key");
            }
            return response;
        }
        private void sortJSONByKey(JObject jObj)
        {
            var props = jObj.Properties().ToList();
            foreach (var prop in props)
            {
                prop.Remove();
            }

            foreach (var prop in props.OrderBy(p => p.Name))
            {
                jObj.Add(prop);
                if (prop.Value is JObject)
                    sortJSONByKey((JObject)prop.Value);
            }
        }
    }
}