using System.Threading.Tasks;
using Refit;
using RM_API_SDK_CSHARP.Model;
using RM_API_SDK_CSHARP.Model.Request;
using RM_API_SDK_CSHARP.Model.Response;
using RM_API_SDK_CSHARP.Model.Transaction;

namespace RM_API_SDK_CSHARP.Service
{
    public interface IPaymentService
    {
        [Get("/v3/payment/transaction/{transactionId}")]
        Task<ApiResponse<ApiResult<Transaction>>> QueryStatusByTransactionID([AliasAs("transactionId")] string transactionId);

        [Get("/v3/payment/transaction/order/{orderId}")]
        Task<ApiResponse<ApiResult<Transaction>>> QueryStatusByOrderID([AliasAs("orderId")] string orderId);

        [Post("/v3/payment/quickpay")]
        Task<ApiResponse<ApiResult<QuickPayResult>>> QuickPay([Body] QuickPayRequest request);
    }
}