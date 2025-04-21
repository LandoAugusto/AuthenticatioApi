using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Application.Interfaces
{
    public  interface IMenuScreenService
    {
        Task<MenuScreenModel?> GetAsync(int code);
    }
}
