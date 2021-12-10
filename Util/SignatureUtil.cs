using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RM_API_SDK_CSHARP.Util
{
    public static class SignatureUtil
    {
        private static byte[] GetSignBytes(string compactJSON, string requestUrl, string nonceStr, string signType, string method, string timestamp)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(compactJSON);
            string encodedData = Convert.ToBase64String(bytes);
            string plainText = "";
            if (compactJSON != "")
            {
                plainText = ("data=" + encodedData + "&method=" + method.ToLower() + "&nonceStr="
                        + nonceStr + "&requestUrl=" + requestUrl + "&signType=" + signType
                        + "&timestamp=" + timestamp);
            }
            else
            {
                plainText = ("method=" + method.ToLower() + "&nonceStr="
                                        + nonceStr + "&requestUrl=" + requestUrl + "&signType=" + signType
                                        + "&timestamp=" + timestamp);
            }
            return Encoding.UTF8.GetBytes(plainText);
        }

        public static string GenerateSignature(this RSACryptoServiceProvider signer, string compactJSON, string requestUrl, string nonceStr, string signType, string method, string timestamp)
        {
            byte[] signature = signer.SignData(GetSignBytes(compactJSON, requestUrl, nonceStr, signType, method, timestamp), CryptoConfig.MapNameToOID(signType));
            return Convert.ToBase64String(signature);
        }

        public static bool VerifySignature(this RSACryptoServiceProvider verifier, string signature, string compactJSON, string requestUrl, string nonceStr, string signType, string method, string timestamp)
        {
            byte[] signBytes = Convert.FromBase64String(signature);
            return verifier.VerifyData(GetSignBytes(compactJSON, requestUrl, nonceStr, signType, method, timestamp), CryptoConfig.MapNameToOID(signType), signBytes);
        }
    }
}
