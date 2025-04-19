using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Core.Entities.Enumrators;
using AuthenticatioApi.Infra.Data.Repositories.Standard.Interfaces;

namespace AuthenticatioApi.Infra.Data.Interfaces
{
    public interface IUserRepository :IDomainRepository<User>   
    {
        Task<User?> GetAsync(int userId, RecordStatusEnum recordStatus);
    }
}
