using AuthenticatioApi.Core.Infrastructure.Configuration;

namespace AuthenticatioApi.Core.Infrastructure.Interfaces
{
    public interface IApiWorkContext
    {
        BaseHeader BaseHeader { get; set; }
    }
}
