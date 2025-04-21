using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Infra.Data.Repositories.Standard.Interfaces;

namespace AuthenticatioApi.Infra.Data.Interfaces
{
    public interface IConfigurationComponentProductRepository : IDomainRepository<ConfigurationComponentProduct>
    {
        Task<ConfigurationComponentProduct?> GetAsync(int code);
    }
}
