using AuthenticatioApi.Core.Entities.Interfaces;
using AuthenticatioApi.Infra.Data.Repositories.Standard.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatioApi.Infra.Data.Repositories.Standard
{
    public class DomainRepository<TEntity> : RepositoryAsync<TEntity>,
                                             IDomainRepository<TEntity> where TEntity : class, IIdentityEntity
    {
        protected DomainRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
