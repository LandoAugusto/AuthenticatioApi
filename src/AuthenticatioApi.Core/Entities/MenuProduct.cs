using AuthenticatioApi.Core.Entities.Interfaces;

namespace AuthenticatioApi.Core.Entities
{
    public class MenuProduct : IIdentityEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int CoverageId { get; set; }
        public int Code { get; set; }
        public int InclusionUserId { get; set; }
        public DateTime InclusionDate { get; set; }
        public int? LastChangeUserId { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public virtual ICollection<MenuScreen> MenuScreen { get; set; } = new HashSet<MenuScreen>();
    }
}
