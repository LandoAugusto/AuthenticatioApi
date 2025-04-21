using System.Security.Claims;

namespace AuthenticatioApi.Infra.Identity.Interfaces
{
    public interface IUser
    {
        string Name { get; }
        int GetUserId();
        string? GetUserEmail();
        string? GetUserName();
        int? GetExternalId();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
    }
}
