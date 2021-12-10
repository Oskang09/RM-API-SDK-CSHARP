using System.Threading.Tasks;
using Refit;
using RM_API_SDK_CSHARP.Model.Request;
using RM_API_SDK_CSHARP.Model.Response;

namespace RM_API_SDK_CSHARP.Service
{
    public interface IOAuthService
    {
        [Headers("Content-Type: application/json")]
        [Post("/v1/token")]
        Task<ApiResponse<OAuthResponse>> GetToken(
            [Header("Authorization")] string token,
            [Body] OAuthRequest request
        );
    }
}
