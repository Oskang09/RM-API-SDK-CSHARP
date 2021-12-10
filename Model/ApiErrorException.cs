namespace RM_API_SDK_CSHARP.Model
{
    [System.Serializable]
    public class ApiErrorException : System.Exception
    {
        public string Code { get; }

        public ApiErrorException() : base("internal sdk error got empty error message")
        {
            this.Code = "sdk-error";
        }

        public ApiErrorException(string message) : base(message)
        {
            this.Code = "sdk-error";
        }

        public ApiErrorException(string errorCode, string message) : base(message)
        {
            this.Code = errorCode;
        }

        public override string ToString()
        {
            return "code: " + this.Code + ", error: " + this.Message.ToString();
        }

    }
}
