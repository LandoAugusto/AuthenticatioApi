using AuthenticatioApi.Core.Entities.Interfaces;

namespace AuthenticatioApi.Core.Entities
{
    public class MenuComponent : IIdentityEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int InclusionUserId { get; set; }
        public DateTime InclusionDate { get; set; }
        public int? LastChangeUserId { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public virtual ICollection<MenuScreen> ConfigurationComponentScreen { get; set; } = new HashSet<MenuScreen>();
    }
}
