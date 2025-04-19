using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationMenuItem
    {
        public ApplicationMenuItem()
        {
            Children = new HashSet<ApplicationMenuItem>();
            RoleMenus = new HashSet<ApplicationRoleMenu>();
        }

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }
        
        public int? Code { get; set; }

        [StringLength(50)]
        public string Url { get; set; }
        
        public virtual ICollection<ApplicationMenuItem> Children { get; set; }

        public virtual ApplicationMenuItem ParentItem { get; set; }

        public virtual ICollection<ApplicationRoleMenu> RoleMenus { get; set; }
    }
}
