using AuthenticatioApi.Core.Entities.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace AuthenticatioApi.Infra.Data.Repositories.Standard
{
    public abstract class SpecificMethods<TEntity> where TEntity : class, IIdentityEntity
    {
        #region ProtectedMethods

        protected abstract IQueryable<TEntity> GenerateQuery(
                    Expression<Func<TEntity, bool>> filter = null,
                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProperties = null);


        protected abstract IEnumerable<TEntity> GetYieldManipulated(IEnumerable<TEntity> entities, Func<TEntity, TEntity> DoAction);
        #endregion ProtectedMethods
    }
}
