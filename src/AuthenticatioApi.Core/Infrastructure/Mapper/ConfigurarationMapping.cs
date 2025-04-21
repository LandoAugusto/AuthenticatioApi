using AuthenticatioApi.Core.Entities;
using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Core.Infrastructure.Mapper
{
    public class ConfigurarationMapping : AutoMapper.Profile
    {
        public ConfigurarationMapping()
        {
            CreateMap<MenuComponentModel, MenuComponent>().ReverseMap();
            CreateMap<MenuProductModel, MenuProduct>().ReverseMap();
            CreateMap<MenuScreenModel, MenuScreen>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
        }
    }
}
