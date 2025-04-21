using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Infra.Data.Contexts;
using AuthenticatioApi.Infra.Data.Interfaces;
using AuthenticatioApi.Infra.Data.Repositories.Standard;
using Microsoft.EntityFrameworkCore;

namespace AuthenticatioApi.Infra.Data.Repositories
{
    internal class ConfigurationComponentProductRepository(AuthenticatioDbContext context) : DomainRepository<ConfigurationComponentProduct>(context), IConfigurationComponentProductRepository
    {
        public async Task<ConfigurationComponentProduct?> GetAsync(int code)
        {
            var query =
                    await Task.FromResult(
                        GenerateQuery(
                            filter: (filtr => filtr.Code.Equals(code)),
                             includeProperties: source =>
                                    source
                                    .Include(item => item.ConfigurationComponentScreen)
                                    .ThenInclude(item => item.ConfigurationComponent),
                            orderBy: item => item.OrderBy(y => y.Id)));

            return query.FirstOrDefault();
        }
    }
}
