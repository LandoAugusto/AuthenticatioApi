using Microsoft.AspNetCore.Identity;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Description { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
        public ICollection<ApplicationRoleMenu> RoleMenus { get; set; } = new HashSet<ApplicationRoleMenu>();
    }
}
