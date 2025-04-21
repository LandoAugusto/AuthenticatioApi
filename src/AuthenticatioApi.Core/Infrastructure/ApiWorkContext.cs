using AuthenticatioApi.Core.Infrastructure.Configuration;
using AuthenticatioApi.Core.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AuthenticatioApi.Core.Infrastructure
{
    /// </summary>
    public class ApiWorkContext : IApiWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public ApiWorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            BaseHeader = GetBaseRequest();
        }

        private string GetHeaderValue(string key)
        {
            _httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(key, out var accept);
            return "";
        }

        /// <summary>
        /// Get Base Request
        /// </summary>
        /// <returns></returns>
        private BaseHeader GetBaseRequest()
        {
            var baseHeader = new BaseHeader
            {
                Accept = GetHeaderValue("accept"),
                AcceptVersion = GetHeaderValue("accept-version"),
                AcceptLanguage = GetHeaderValue("accept-language"),
                Authorization = GetHeaderValue("authorization")
            };

            if (string.IsNullOrEmpty(baseHeader.Authorization))
                baseHeader.Authorization = GetHeaderValue("Authorization");

            baseHeader.AmcApplication = GetHeaderValue("x-application")?.ToUpper();
            baseHeader.CorrelationId = GetHeaderValue("x-correlation-id");
            baseHeader.ClientId = GetHeaderValue("x-client-id");
            baseHeader.ClientIp = GetHeaderValue("x-client-ip");

            return baseHeader;
        }

        /// <summary>
        /// Base header
        /// </summary>
        public BaseHeader BaseHeader { get; set; }
    }
}
