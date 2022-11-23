namespace net_api.Data
{
    public class RefreshToken
    {

        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; }
    }
}
