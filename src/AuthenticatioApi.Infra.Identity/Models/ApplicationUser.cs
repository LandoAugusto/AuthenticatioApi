using Microsoft.AspNetCore.Identity;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationUser : IdentityUser<int>
    {      
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = new HashSet<ApplicationUserClaim>();
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = new HashSet<ApplicationUserLogin>();
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = new HashSet<ApplicationUserToken>();
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new HashSet<ApplicationUserRole>();
    }
}
