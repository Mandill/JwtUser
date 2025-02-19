namespace JwtUser.Helper
{
    public class APIResponse<T>
    {
        public APIResponse()
        {
            this.ResponseCode = 400;
            this.Result = typeof(T) == typeof(List<object>) ? (T)(object)new List<object>() : default!;
        }
        public int ResponseCode { get; set; }
        public T Result { get; set; } 
        public string Message { get; set; } = string.Empty;
    }
}
