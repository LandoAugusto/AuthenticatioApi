using AuthenticatioApi.Application.Interfaces;
using AuthenticatioApi.Controllers.V1.Base;
using AuthenticatioApi.Core.Entities.Enumrators;
using AuthenticatioApi.Core.Infrastructure.Configuration;
using AuthenticatioApi.Core.Model;
using AuthenticatioApi.Core.Models.Auth;
using AuthenticatioApi.Infra.Identity.Interfaces;
using AuthenticatioApi.Infra.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthenticatioApi.Core.Infrastructure.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticatioApi.Controllers.V1
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly ILogger logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApiConfig apiConfig;
        private readonly IUserService userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenConfiguration"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public AuthController(IOptions<ApiConfig> tokenConfiguration, IUser user, ILogger<AuthController> logger, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IUserService userService)
        {
            this.logger = logger;
            this.apiConfig = tokenConfiguration.Value;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService; 
        }

        [AllowAnonymous]
        [HttpPost("token")]
        [ProducesResponseType(typeof(BaseDataResponseModel<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseDataResponseModel<>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseDataResponseModel<>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> TokenRequestAsync(TokenRequest request)
        {
            var result = await signInManager.PasswordSignInAsync(request.Login, request.Password, false, true);
            if (result.IsLockedOut)
            {
                throw new BusinessException("Usuário temporariamente bloqueado por tentativas inválidas.");
            }
            if (result.Succeeded)
            {
                var jwt = await GerarJwt(request.Login);
                return base.ReturnSuccess(jwt);
            }

            throw new BusinessException("Usuário ou senha incorretos.");

        }

        private async Task<TokenResponse> GerarJwt(string login)
        {
            var userManager = await this.userManager.FindByNameAsync(login);
            var claims = await this.userManager.GetClaimsAsync(userManager);
            var userRoles = await this.userManager.GetRolesAsync(userManager);

            var user = await userService.GetAsync(userManager.Id, RecordStatusEnum.Ativo);
            claims.Add(new Claim("userId", user.Id.ToString()));            
            claims.Add(new Claim("userName", user.Name));
            claims.Add(new Claim("profileId", user.ProfileId.ToString()));
            claims.Add(new Claim("code", user.Code.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, userManager.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(apiConfig.Jwt.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = apiConfig.Jwt.Issuer,
                Audience = apiConfig.Jwt.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(apiConfig.Jwt.ExpiresInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new TokenResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(apiConfig.Jwt.ExpiresInMinutes).TotalSeconds,
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
