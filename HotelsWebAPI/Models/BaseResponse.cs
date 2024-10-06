namespace HotelsWebAPI.Models
{
    public class BaseResponse<T>
    {
        public T? Value { get; set; }
        public required int StatusCode { get; set; }
        public required string Message { get; set; }

    }
}
