using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticatioApi.Infra.Identity.Models
{
    public class ApplicationRoleMenu
    {
        

        [Key, Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Column(Order = 2)]
        public virtual int RoleId { get; set; }

        [Column(Order = 3)]
        public virtual int MenuId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationMenuItem MenuItem { get; set; }
    }
}
