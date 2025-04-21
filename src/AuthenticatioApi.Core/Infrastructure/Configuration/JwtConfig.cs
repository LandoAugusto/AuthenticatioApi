namespace AuthenticatioApi.Core.Infrastructure.Configuration
{
    public class JwtConfig
    {
        public bool Enable { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; internal set; } = "G[>R]0vnsY^bA/O`y6ru#8cZgm(GUv0TxG;!JI6-7`EI/9&KR/4``g5rmc:!\"Jo";
        public int ExpiresInMinutes { get; set; }
    }
}
