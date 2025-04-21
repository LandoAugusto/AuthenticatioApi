using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Controllers.V1.Base;
using AuthenticatioApi.Core.Model;
using AuthenticatioApi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticatioApi.Controllers.V1
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configurationComponentScreenService"></param>
    public class ComponentController(IMenuScreenAppService configurationComponentScreenService) : BaseController
    {
        private readonly IMenuScreenAppService _configurationComponentScreenService = configurationComponentScreenService;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("list-menu-screen/{code}")]
        [ProducesResponseType(typeof(BaseDataResponseModel<MenuScreenModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseDataResponseModel<MenuScreenModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseDataResponseModel<MenuScreenModel>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListAsync(int code)
        {
            var response = await _configurationComponentScreenService.GetAsync(code);
            if (response == null)
                return ReturnNotFound();

            return base.ReturnSuccess(response);
        }
    }
}
