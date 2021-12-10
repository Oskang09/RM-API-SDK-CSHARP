namespace RM_API_SDK_CSHARP.Model
{
    public class ApiResult<T>
    {
        public string Code { get; set; }
        public T Item { get; set; }
        public ErrorResult Error { get; set; }
    }
}
