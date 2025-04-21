using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Infra.Data.Repositories.Standard.Interfaces;

namespace AuthenticatioApi.Infra.Data.Interfaces
{
    public interface IMenuProductRepository : IDomainRepository<MenuProduct>
    {
        Task<MenuProduct?> GetAsync(int code);
    }
}
