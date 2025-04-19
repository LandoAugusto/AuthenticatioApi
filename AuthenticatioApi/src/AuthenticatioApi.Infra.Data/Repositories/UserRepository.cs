using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Core.Entities.Enumrators;
using AuthenticatioApi.Infra.Data.Contexts;
using AuthenticatioApi.Infra.Data.Interfaces;
using AuthenticatioApi.Infra.Data.Repositories.Standard;

namespace AuthenticatioApi.Infra.Data.Repositories
{
    internal class UserRepository(AuthenticatioDbContext context) : DomainRepository<User>(context), IUserRepository
    {
        public async Task<User?> GetAsync(int userId, RecordStatusEnum recordStatus)
        {
            var query =
                    await Task.FromResult(
                        GenerateQuery(
                            filter: (filtr => filtr.UserId.Equals(userId)                                                
                                                && filtr.Status.Equals((int)recordStatus)),
                            orderBy: item => item.OrderBy(y => y.UserId)));

            return query.FirstOrDefault();
        }
    }
}
