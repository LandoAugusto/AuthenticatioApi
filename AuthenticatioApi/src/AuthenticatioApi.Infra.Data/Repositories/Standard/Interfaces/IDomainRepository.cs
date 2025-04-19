using AuthenticatioApi.Core.Entities.Interfaces;

namespace AuthenticatioApi.Infra.Data.Repositories.Standard.Interfaces
{
    public interface IDomainRepository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class, IIdentityEntity
    {
    }
}
