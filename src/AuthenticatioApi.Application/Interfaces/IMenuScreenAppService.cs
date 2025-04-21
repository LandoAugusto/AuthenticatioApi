using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Application.Interfaces
{
    public  interface IMenuScreenAppService
    {
        Task<MenuScreenModel?> GetAsync(int code);
    }
}
