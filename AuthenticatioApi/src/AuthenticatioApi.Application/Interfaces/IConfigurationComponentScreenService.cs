using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Application.Interfaces
{
    public  interface IConfigurationComponentScreenService
    {
        Task<ConfigurationComponentScreenModel?> GetAsync(int code);
    }
}
