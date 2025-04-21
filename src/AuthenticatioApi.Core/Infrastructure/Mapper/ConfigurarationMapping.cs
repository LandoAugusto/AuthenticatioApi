using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Core.Infrastructure.Mapper
{
    public class ConfigurarationMapping : AutoMapper.Profile
    {
        public ConfigurarationMapping()
        {
            CreateMap<ConfigurationComponentModel, ConfigurationComponent>().ReverseMap();
            CreateMap<ConfigurationComponentProductModel, ConfigurationComponentProduct>().ReverseMap();
            CreateMap<ConfigurationComponentScreenModel, ConfigurationComponentScreen>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
