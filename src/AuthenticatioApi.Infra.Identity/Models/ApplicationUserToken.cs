using Microsoft.AspNetCore.Identity;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationUserToken : IdentityUserToken<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}