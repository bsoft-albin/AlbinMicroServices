namespace AlbinMicroService.Core
{
    public class ApiResponse<X> where X : class, new()
    {
        public short StatusCode { get; set; } = 200;
        public string StatusMessage { get; set; } = "OK";
        public X Data { get; set; } = new X();
    }
}
