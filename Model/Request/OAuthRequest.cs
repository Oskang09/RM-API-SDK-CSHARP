using Newtonsoft.Json;

namespace RM_API_SDK_CSHARP.Model.Request
{
    public class OAuthRequest
    {
        public OAuthRequestGrantType GrantType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
    }
}
