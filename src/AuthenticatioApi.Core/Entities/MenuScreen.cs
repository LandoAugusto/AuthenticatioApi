using AuthenticatioApi.Core.Entities.Interfaces;

namespace AuthenticatioApi.Core.Entities
{
    public class MenuScreen : IIdentityEntity
    {
        public int Id { get; set; }
        public int MenuProductId { get; set; }
        public int MenuComponentId { get; set; }
        public int Order { get; set; }
        public int InclusionUserId { get; set; }
        public DateTime InclusionDate { get; set; }
        public int? LastChangeUserId { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public virtual MenuComponent MenuComponent { get; set; } = null!;
        public virtual MenuProduct MenuProduct { get; set; } = null!;
    }
}
