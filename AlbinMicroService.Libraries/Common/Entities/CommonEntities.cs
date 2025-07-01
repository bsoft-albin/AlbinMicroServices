namespace AlbinMicroService.Libraries.Common.Entities
{
    public class TokenResponse
    {
        public string? AccessToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class TokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
    }
    public class RefreshTokenResult
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RefreshTokenPayload : RefreshTokenResult
    {
        public Dictionary<string, string> RefreshPayload { get; set; } = [];
    }
}
