using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Infra.Data.Contexts;
using AuthenticatioApi.Infra.Data.Interfaces;
using AuthenticatioApi.Infra.Data.Repositories.Standard;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatioApi.Infra.Data.Repositories
{
    internal class MenuProductRepository(AuthenticatioDbContext context) : DomainRepository<MenuProduct>(context), IMenuProductRepository
    {
        public async Task<MenuProduct?> GetAsync(int code)
        {
            var query =
                    await Task.FromResult(
                        GenerateQuery(
                            filter: (filtr => filtr.Code.Equals(code)),
                             includeProperties: source =>
                                    source
                                    .Include(item => item.MenuScreen)
                                    .ThenInclude(item => item.MenuComponent),
                            orderBy: item => item.OrderBy(y => y.Id)));

            return query.FirstOrDefault();
        }
    }
}
