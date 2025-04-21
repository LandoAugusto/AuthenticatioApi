using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Core.Models;
using AuthenticatioApi.Infra.Data.Interfaces;
using AutoMapper;

namespace AuthenticatioApi.Application.Services
{
    internal class ConfigurationComponentScreenService(IMapper mapper, IConfigurationComponentProductRepository configurationComponentProductRepository) : IConfigurationComponentScreenService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IConfigurationComponentProductRepository _configurationComponentProductRepository = configurationComponentProductRepository;

        public async Task<ConfigurationComponentScreenModel?> GetAsync(int code)
        {
            var entidade = await _configurationComponentProductRepository.GetAsync(code);
            if (entidade == null) return null;
            var response = new ConfigurationComponentScreenModel()
            {
                Product = _mapper.Map<ConfigurationComponentProductModel>(entidade)
            };

            foreach (var item in entidade.ConfigurationComponentScreen)
            {
                var configurationComponentModel = _mapper.Map<ConfigurationComponentModel>(item.ConfigurationComponent);
                configurationComponentModel.Order = item.Order;
                response.Component.Add(configurationComponentModel);   
            }

            return response;
        }
    }
}
