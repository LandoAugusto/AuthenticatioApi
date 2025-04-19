using Microsoft.AspNetCore.Identity;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
