using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;
using RM_API_SDK_CSHARP.Model;
using RM_API_SDK_CSHARP.Service;
using RM_API_SDK_CSHARP.Model.Response;
using RM_API_SDK_CSHARP.Model.Request;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using RM_API_SDK_CSHARP.Extension;
using RM_API_SDK_CSHARP.Util;
using System.Net.Http;

namespace RM_API_SDK_CSHARP
{
    public class RevenueMonsterOpenAPI
    {
        public class OAuthHandler
        {
            private RevenueMonsterOpenAPI rmOpenApi;
            private string token;

            public String AccessToken;
            public String TokenType;
            public String RefreshToken;

            public OAuthHandler(RevenueMonsterOpenAPI api)
            {
                this.rmOpenApi = api;

                byte[] bytes = Encoding.UTF8.GetBytes(this.rmOpenApi.clientId + ":" + this.rmOpenApi.clientSecret);
                this.token = Convert.ToBase64String(bytes);
            }

            public void UpdateToken(String accessToken, String refreshToken, String tokenType = "BEARER")
            {
                this.AccessToken = accessToken;
                this.RefreshToken = refreshToken;
                this.TokenType = tokenType;
            }

            public Task<ApiResponse<OAuthResponse>> Authenticate()
            {
                return this.rmOpenApi.oAuthService.GetToken(this.token, new OAuthRequest
                {
                    GrantType = OAuthRequestGrantType.client_credentials,
                });
            }

            public Task<ApiResponse<OAuthResponse>> AuthenticateWithRefreshToken()
            {
                return this.rmOpenApi.oAuthService.GetToken(this.token, new OAuthRequest
                {
                    GrantType = OAuthRequestGrantType.refresh_token,
                    RefreshToken = this.RefreshToken,
                });
            }

            public Timer AutoAuthenticate(String accessToken, String refreshToken, Action<OAuthResponse, Exception> onRefresh = null, int interval = 30 * 60 * 1000, String tokenType = "BEARER")
            {
                this.AccessToken = accessToken;
                this.RefreshToken = refreshToken;
                this.TokenType = tokenType;

                return new Timer(
                    async delegate (object state)
                    {
                        try
                        {
                            OAuthResponse token;
                            if (this.RefreshToken == "")
                            {
                                token = await this.rmOpenApi.EnsureResponse(Authenticate());
                            }
                            else
                            {
                                token = await this.rmOpenApi.EnsureResponse(AuthenticateWithRefreshToken());
                            }

                            if (onRefresh != null)
                            {
                                onRefresh.Invoke(token, null);
                            }

                            this.AccessToken = token.AccessToken;
                            this.RefreshToken = token.RefreshToken;
                            this.TokenType = token.TokenType;
                        }
                        catch (Exception error)
                        {
                            if (onRefresh != null)
                            {
                                onRefresh.Invoke(null, error);
                            }
                        }
                    },
                    null, 0,
                    interval
                );
            }
        }

        private Dictionary<EnvironmentTarget, String> oauthUrls = new Dictionary<EnvironmentTarget, string>
        {
            {EnvironmentTarget.SANDBOX, "https://sb-oauth.revenuemonster.my/"},
            {EnvironmentTarget.PRODUCTION, "https://oauth.revenuemonster.my/"},
        };

        private Dictionary<EnvironmentTarget, String> openApiUrls = new Dictionary<EnvironmentTarget, string>
        {
            {EnvironmentTarget.SANDBOX, "https://sb-open.revenuemonster.my/"},
            {EnvironmentTarget.PRODUCTION, "https://open.revenuemonster.my/"},
        };

        private EnvironmentTarget environment;
        private String clientId;
        private String clientSecret;
        private String privateKey;
        private String publicKey;
        private string signType;
        private IOAuthService oAuthService;
        private RSACryptoServiceProvider signer;
        private RSACryptoServiceProvider verifier;
        public JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new JSONContractResolver(),
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ",
            Converters = new List<JsonConverter>()
            {
                new Newtonsoft.Json.Converters.StringEnumConverter(new DefaultNamingStrategy(), false),
            }
        };

        public OAuthHandler OAuth;
        public IPaymentService Payment;

        public RevenueMonsterOpenAPI(EnvironmentTarget environment, String clientId, String clientSecret, String privateKey, String publicKey, String signType = "sha256")
        {
            this.environment = environment;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.privateKey = privateKey;
            this.publicKey = publicKey;
            this.signType = signType;
        }

        public void Initialize()
        {
            signer = PemKeyUtil.GetRSAProviderFromPemString(this.privateKey);
            verifier = PemKeyUtil.GetRSAProviderFromPemString(this.publicKey);

            this.oAuthService = RestService.For<IOAuthService>(this.getOAuthURL(), new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(this.jsonSerializerSettings)
            });
            this.OAuth = new OAuthHandler(this);
        }

        public void SetupOpenApiSdk()
        {
            this.Payment = RestService.For<IPaymentService>(this.getOpenAPIURL(), new RefitSettings
            {
                HttpMessageHandlerFactory = () => new OpenAPIInterceptor(this, signer, verifier, signType),
                ContentSerializer = new NewtonsoftJsonContentSerializer(this.jsonSerializerSettings),
            });
        }

        public HttpClient UseOpenApiHttpClient()
        {
            return RestService.CreateHttpClient(this.getOpenAPIURL(), new RefitSettings
            {
                HttpMessageHandlerFactory = () => new OpenAPIInterceptor(this, signer, verifier, signType),
                ContentSerializer = new NewtonsoftJsonContentSerializer(this.jsonSerializerSettings),
            });
        }

        public async Task<T> EnsureResponse<T>(Task<ApiResponse<T>> result)
        {
            ApiResponse<T> response;
            try
            {
                response = await result;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                throw new ApiErrorException(error.Message.ToString());
            }

            if (response.IsSuccessStatusCode)
            {
                if (response.Content is ApiResult<T>)
                {
                    var openapiResp = response.Content as ApiResult<T>;
                    if (openapiResp.Error != null)
                    {
                        throw new ApiErrorException(openapiResp.Error.Code, openapiResp.Error.Message);
                    }
                    return openapiResp.Item;
                }
                return response.Content;
            }
            else
            {
                var errorResp = JsonConvert.DeserializeObject<ApiResult<T>>(response.Error.Content, jsonSerializerSettings);
                if (errorResp != null)
                {
                    throw new ApiErrorException(errorResp.Error.Code, errorResp.Error.Message);
                }
                throw new ApiErrorException(response.StatusCode.ToString(), response.ReasonPhrase);
            }
        }

        public async Task<T> EnsureResponse<T>(HttpClient client, HttpMethod method, string path, object content = null)
        {
            HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(method, path);
                if (content != null)
                {
                    request.Content = new StringContent(
                        JsonConvert.SerializeObject(content, jsonSerializerSettings),
                        Encoding.UTF8, "application/json"
                    );
                }
                response = await client.SendAsync(request);
            }
            catch (Exception error)
            {
                throw new ApiErrorException(error.Message.ToString());
            }

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResult<T>>(json, jsonSerializerSettings);
                if (apiResponse == null || apiResponse.Item == null)
                {
                    return default(T);
                }
                return apiResponse.Item;
            }
            else
            {
                var errorResp = JsonConvert.DeserializeObject<ApiResult<object>>(await response.Content.ReadAsStringAsync(), jsonSerializerSettings);
                if (errorResp != null)
                {
                    throw new ApiErrorException(errorResp.Error.Code, errorResp.Error.Message);
                }
                throw new ApiErrorException(response.StatusCode.ToString(), response.ReasonPhrase);
            }
        }

        private String getOAuthURL()
        {
            return oauthUrls[this.environment];
        }

        private String getOpenAPIURL()
        {
            return openApiUrls[this.environment];
        }

    }

}
