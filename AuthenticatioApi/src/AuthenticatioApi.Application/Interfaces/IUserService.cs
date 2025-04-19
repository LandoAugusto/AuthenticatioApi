using AuthenticatioApi.Core.Entities.Enumrators;
using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserModel?> GetAsync(int userId, RecordStatusEnum recordStatus);
    }
}
