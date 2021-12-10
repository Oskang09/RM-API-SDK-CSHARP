using System;
using System.Linq;

namespace RM_API_SDK_CSHARP.Util
{
    public static class RandomUtil
    {
        private static Random random = new Random();

        public static string RandomString(this Int32 length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
