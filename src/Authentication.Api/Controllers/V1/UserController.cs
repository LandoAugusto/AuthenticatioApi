using AuthenticatioApi.Controllers.V1.Base;
using AuthenticatioApi.Core.Infrastructure.Exceptions;
using AuthenticatioApi.Core.Model;
using AuthenticatioApi.Core.Models;
using AuthenticatioApi.Infra.Identity.Context;
using AuthenticatioApi.Infra.Identity.Interfaces;
using AuthenticatioApi.Infra.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Api.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IdentityDbContext dbContext;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="rolemanager"></param>
        /// <param name="dbContext"></param>
        public UserController(
            IUser user,
            ILogger<UserController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> rolemanager,
            IdentityDbContext dbContext)
        {
            (this.signInManager, this.userManager, this.roleManager, this.dbContext) = (signInManager, userManager, rolemanager, dbContext);
        }

        [HttpPost]
        [Route("get-users")]
        public async Task<ActionResult> GetAsync(UserFilterModel request)
        {
            try
            {
                ApplicationUser existentUser = null;
                if (!string.IsNullOrWhiteSpace(request.Id))
                {
                    existentUser = await userManager.FindByIdAsync(request.Id);
                }
                if (existentUser == null && !string.IsNullOrWhiteSpace(request.Login))
                {
                    existentUser = await userManager.FindByNameAsync(request.Login);
                }
                if (existentUser == null && !string.IsNullOrWhiteSpace(request.Email))
                {
                    existentUser = await userManager.FindByEmailAsync(request.Email);
                }
                if (existentUser == null) ReturnNotFound();

                var users = await userManager.Users.Select(u => new
                {
                    existentUser.Id,
                    Name = existentUser.UserName,
                    existentUser.Email,
                    Roles = new List<RoleResponse>(),
                }).ToListAsync();


                var roles = await roleManager.Roles.Select(r => new { r.Id, r.Name, r.Description }).ToListAsync();

                foreach (var role in roles)
                {
                    var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                    var toUpdate = users.Where(u => usersInRole.Any(ur => ur.Id == existentUser.Id));

                    foreach (var user in toUpdate)
                    {
                        user.Roles.Add(new RoleResponse(role.Id, role.Name, role.Description));
                    }
                }

                return base.ReturnSuccess(users);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-user/{userId}")]
        public async Task<ActionResult> GetUserAsync(string userId)
        {
            try
            {
                var existentUser = await userManager.FindByIdAsync(userId);

                var users = await userManager.Users.Select(u => new
                {
                    existentUser.Id,
                    Name = existentUser.UserName,
                    existentUser.Email,
                    Roles = new List<RoleResponse>(),
                }).ToListAsync();


                var roles = await roleManager.Roles.Select(r => new { r.Id, r.Name, r.Description }).ToListAsync();

                foreach (var role in roles)
                {
                    var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                    var toUpdate = users.Where(u => usersInRole.Any(ur => ur.Id == existentUser.Id));

                    foreach (var user in toUpdate)
                    {
                        user.Roles.Add(new RoleResponse(role.Id, role.Name, role.Description));
                    }
                }

                return base.ReturnSuccess(users.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

      
        [HttpPost("save-user")]
        public async Task<ActionResult> SaveAsync(SaveUserRequest model)
        {
            try
            {
                var existentUser = await userManager.FindByNameAsync(model.Login);
                if (existentUser != null)
                {
                    throw new BusinessException($"Já existe outro usuário cadastrado com o login '{model.Login}'.");
                }

                var roleNames = await roleManager.FindByIdAsync(model.RoleId.ToString())
                    ?? throw new BusinessException($"Role '{model.RoleId}' não foi lozalizada.");

                var user = new ApplicationUser
                {
                    UserName = model.Login,
                    Email = model.Email,
                    EmailConfirmed = true,
                };

                var createdUser = await userManager.CreateAsync(user, model.Password);
                if (createdUser.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    var roleResult = await userManager.AddToRoleAsync(user, roleNames.Name);
                }

                return base.ReturnSuccess(user.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-user")]
        public async Task<ActionResult> UpdateAsync(UpdateUserRequest model)
        {
            try
            {
                var existentUser = await userManager.FindByIdAsync(model.Id)
                    ?? throw new BusinessException($"Usuário com identificador '{model.Id}' não localizado.");

                var users = await userManager.Users.Select(u => new
                {
                    existentUser.Id,
                    Name = existentUser.UserName,
                    existentUser.Email,
                    Roles = new List<RoleResponse>()
                }).ToListAsync();

                var roles = await roleManager.Roles.Select(r => new { r.Id, r.Name, r.Description }).ToListAsync();
                foreach (var role in roles)
                {
                    var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
                    var toUpdate = users.Where(u => usersInRole.Any(ur => ur.Id == existentUser.Id)).ToList();

                    foreach (var userRole in toUpdate)
                    {
                        userRole.Roles.Add(new RoleResponse(role.Id, role.Name, role.Description));
                    }
                }

                var roleNames = await roleManager.FindByIdAsync(model.RoleId.ToString())
                    ?? throw new BusinessException($"Role '{model.RoleId}' não foi lozalizada.");

                existentUser.Email = model.Email;

                var x = users.FirstOrDefault();

                var result = await userManager.UpdateAsync(existentUser);
                if (result.Succeeded)
                {
                    var oldRoleId = existentUser.UserRoles.Select(x => x.RoleId).FirstOrDefault();
                    var oldRoleName = await dbContext.Roles.SingleOrDefaultAsync(r => r.Id == oldRoleId);
                    await userManager.RemoveFromRoleAsync(existentUser, x.Roles.FirstOrDefault().Name);
                    await userManager.AddToRoleAsync(existentUser, roleNames.Name);
                }

                return base.ReturnSuccess(true);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-password")]
        public async Task<ActionResult> UpdatePasswordAsync(UpdatePasswordUserRequest model)
        {
            try
            {
                var existentUser = await userManager.FindByIdAsync(model.Id.ToString())
                    ?? throw new BusinessException($"Usuário com identificador '{model.Id}' não localizado.");

                var result = await userManager.ChangePasswordAsync(existentUser, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception(error.Description);
                    }
                }

                return base.ReturnSuccess(true);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("updated-status")]
        public async Task<ActionResult> UpdateStatusAsync(UpdateStatusUserRequest model)
        {

            try
            {
                var existentUser = await userManager.FindByIdAsync(model.Id) ?? throw new BusinessException($"Usuário com identificador '{model.Id}' não localizado.");

                var result = await userManager.SetLockoutEnabledAsync(existentUser, true);
                if (result.Succeeded)
                {
                    result = await userManager.SetLockoutEndDateAsync(existentUser, DateTime.MaxValue.Date);
                }
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception(error.Description);
                    }
                }

                return base.ReturnSuccess(true);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("get-role-by-id/{roleId}")]
        public async Task<ActionResult> ListRoles(int roleId)
        {
            var role = await roleManager.Roles
                        .Where(item => item.Id == roleId)
                        .Include(item => item.RoleMenus)
                        .FirstOrDefaultAsync();

            var MenuIds = new List<int>();

            foreach (var roleMenu in role.RoleMenus)
                MenuIds.Add(roleMenu.MenuId);

            return base.ReturnSuccess(new RoleResponse(role.Id, role.Name, role.Description, MenuIds));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("get-roles")]
        public async Task<ActionResult> GetRoles(RoleFilterModel request)
        {
            var role = await roleManager.Roles
                        .Where(item => (request.Id == null || item.Id == request.Id)
                        && (string.IsNullOrEmpty(request.Name) || item.Name == request.Name)).ToListAsync();

            return base.ReturnSuccess(role.Select(item =>
            {
                return new RoleResponse(item.Id, item.Name, item.Description);
            }).ToList());
        }

        [HttpPost("save-role")]
        public async Task<ActionResult> CreateRoles(SaveRoleRequest request)
        {
            IdentityResult roleResult;

            if (request.Id is not null)
            {
                var role = await roleManager.Roles.Where(item => item.Id == request.Id).FirstOrDefaultAsync() ??
                    throw new BusinessException($"Perfil com identificador '{request.Id}' não localizado.");

                role.Name = request.Name;
                role.Description = request.Description;

                roleResult = await roleManager.UpdateAsync(role);
            }
            else
            {
                var isExists = await roleManager.RoleExistsAsync(request.Name);
                if (isExists)
                    throw new BusinessException($"Já existe role '{request.Name}' cadastrada.");

                roleResult = await roleManager.CreateAsync(new ApplicationRole(request.Name, request.Description));
            }

            return base.ReturnSuccess(request.Name);

        }
    }
}
