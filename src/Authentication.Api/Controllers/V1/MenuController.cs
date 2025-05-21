using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Controllers.V1.Base;
using AuthenticatioApi.Core.Infrastructure.Exceptions;
using AuthenticatioApi.Core.Model;
using AuthenticatioApi.Core.Models;
using AuthenticatioApi.Infra.Identity.Context;
using AuthenticatioApi.Infra.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Api.Controllers.V1
{

   /// <summary>
   /// 
   /// </summary>   
   /// <param name="dbContext"></param>
   /// <param name="rolemanager"></param>
   /// <param name="menuScreenAppService"></param>
    public class MenuController(IdentityDbContext dbContext, RoleManager<ApplicationRole> rolemanager, IMenuScreenAppService menuScreenAppService) 
        : BaseController
    {

        private readonly IdentityDbContext _dbContext = dbContext;
        private readonly RoleManager<ApplicationRole> roleManager = rolemanager;


        private readonly IMenuScreenAppService _menuScreenAppService = menuScreenAppService;


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
            var response = await _menuScreenAppService.GetAsync(code);
            if (response == null)
                return ReturnNotFound();

            return base.ReturnSuccess(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpGet]
        [Route("get-role-menu")]
        public async Task<ActionResult> GetAsync([FromQuery, Required] string roleName)
        {
            var role = await roleManager.Roles.Where(x => x.Name == roleName).FirstOrDefaultAsync()
             ?? throw new BusinessException($"Role '{roleName}' não foi lozalizada.");

            var query = (from a in _dbContext.RoleMenus
                         join b in _dbContext.MenuItems on a.MenuId equals b.Id
                         where a.RoleId == role.Id
                         select b).ToList();

            var response = new ListMenuModel()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
            };
            var menuItem = query.Where(x => x.ParentId == null).ToList();
            foreach (var item in menuItem)
            {
                var menu = new MenuModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Url = item.Url,
                    Icon = item.Icon,
                    Code = item.Code
                };

                foreach (var itemMenu in query.Where(x => x.ParentId == item.Id))
                {
                    menu.MenuItem?.Add(new MenuModel()
                    {
                        Id = itemMenu.Id,
                        Title = itemMenu.Title,
                        Icon = itemMenu.Icon,
                        Url = itemMenu.Url,
                        Code = itemMenu.Code,
                        MenuItem = query.Where(item => item.ParentId == itemMenu.Id).Select(item => new MenuModel()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Icon = item.Icon,
                            Url = item.Url,
                            Code = itemMenu.Code,

                        }).ToList()
                    });
                }
                ;
                response.Menus?.Add(menu);
            }
            return ReturnSuccess(response);
        }
    }
}
