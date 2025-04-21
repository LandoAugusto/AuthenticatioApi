using AuthenticatioApi.Infra.Identity.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor) => _accessor = accessor;

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public int GetUserId() => IsAuthenticated() ? int.Parse(_accessor.HttpContext.User.GetUserId()) : default;

        public string? GetUserEmail() => IsAuthenticated() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;

        public string? GetUserName() => IsAuthenticated() ? _accessor.HttpContext.User.GetUserName() : string.Empty;

        public int? GetExternalId() => IsAuthenticated() ? _accessor.HttpContext.User.GetExtenalID() : null;

        public bool IsAuthenticated() => _accessor.HttpContext.User.Identity.IsAuthenticated;

        public bool IsInRole(string role) => _accessor.HttpContext.User.IsInRole(role);

        public IEnumerable<Claim> GetClaimsIdentity() => _accessor.HttpContext.User.Claims;

    }

    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(null, nameof(principal));
            }
            var claim = principal.FindFirst("userId");
            if (claim == null) return null;
            return claim?.Value;
        }

        public static string? GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(null, nameof(principal));
            }
            var claim = principal.FindFirst(ClaimTypes.Email);
            if (claim == null) return null;
            return claim.Value;
        }

        public static string? GetUserName(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(null, nameof(principal));
            }
            var claim = principal.FindFirst("userName");
            if (claim == null) return null;
            return claim.Value;
        }

        public static int? GetExtenalID(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(null, nameof(principal));
            }
            var claim = principal.FindFirst("extID");
            if (claim == null) return null;
            return int.Parse(claim.Value);
        }
    }
}
