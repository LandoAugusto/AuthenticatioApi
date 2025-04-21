using Newtonsoft.Json;

namespace AuthenticatioApi.Core.Infrastructure.Configuration
{
    public class BaseHeader
    {
        [JsonProperty("accept")]
        public string Accept { get; set; }

        [JsonProperty("accept-version")]
        public string AcceptVersion { get; set; }

        [JsonProperty("accept-language")]
        public string AcceptLanguage { get; set; }

        [JsonProperty("authorization")]
        public string Authorization { get; set; }

        [JsonProperty("content-type")]
        public string ContentType { get; set; }

        [JsonProperty("user-agent")]
        public string UserAgent { get; set; }

        [JsonProperty("Pragma")]
        public string Pragma { get; set; }

        [JsonProperty("Cache-Control")]
        public string CacheControl { get; set; }

        [JsonProperty("x-application")]
        public string AmcApplication { get; set; }

        [JsonProperty("amc-message-id")]
        public string AmcMessageId { get; set; }

        [JsonProperty("amc-session-id")]
        public string AmcSessionId { get; set; }

        [JsonProperty("amc-work-id")]
        public string AmcWorkId { get; set; }

        [JsonProperty("client-ip")]
        public string ClientIp { get; set; }

        [JsonProperty("Client-ID")]
        public string ClientId { get; set; }

        [JsonProperty("x-correlation-id")]
        public string CorrelationId { get; set; }

        [JsonProperty("buuid")]
        public string Buuid { get; set; }

        [JsonProperty("bcsid")]
        public string Bcsid { get; set; }

        [JsonProperty("Access-Control-Allow-Origin")]
        public string AccessControlAllowOrigin { get; set; }

        [JsonProperty("Access-Control-Expose-Headers")]
        public string AccessControlExposeHeaders { get; set; }

        [JsonProperty("Content-Length")]
        public string ContentLength { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }
    }
}
