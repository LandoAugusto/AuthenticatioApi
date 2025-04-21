using AuthenticatioApi.Core.Models;

namespace AuthenticatioApi.Core.Models
{
    public class ConfigurationComponentScreenModel
    {   
        public ConfigurationComponentProductModel Product { get; set; }  

        public List<ConfigurationComponentModel> Component { get; set; } = [];

    }
}
