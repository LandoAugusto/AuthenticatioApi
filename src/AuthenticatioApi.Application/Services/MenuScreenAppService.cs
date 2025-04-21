using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Core.Models;
using AuthenticatioApi.Infra.Data.Interfaces;
using AutoMapper;

namespace AuthenticatioApi.Application.Services
{
    internal class MenuScreenAppService(IMapper mapper, IMenuProductRepository menuProductRepository) : IMenuScreenAppService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMenuProductRepository _menuProductRepository = menuProductRepository;

        public async Task<MenuScreenModel?> GetAsync(int code)
        {
            var entidade = await _menuProductRepository.GetAsync(code);
            if (entidade == null) return null;
            var response = new MenuScreenModel()
            {
                Product = _mapper.Map<MenuProductModel>(entidade)
            };

            foreach (var item in entidade.MenuScreen)
            {
                var configurationComponentModel = _mapper.Map<MenuComponentModel>(item.MenuComponent);
                configurationComponentModel.Order = item.Order;
                response.Component.Add(configurationComponentModel);   
            }

            return response;
        }
    }
}
