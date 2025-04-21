using AuthenticatioApi.Core.Entities.Interfaces;

namespace AuthenticatioApi.Core.Entities
{
    public class ConfigurationComponentScreen : IIdentityEntity
    {
        public int Id { get; set; }
        public int ConfigurationComponentProductId { get; set; }
        public int ConfigurationComponentId { get; set; }
        public int Order { get; set; }
        public int InclusionUserId { get; set; }
        public DateTime InclusionDate { get; set; }
        public int? LastChangeUserId { get; set; }
        public DateTime? LastChangeDate { get; set; }
        public virtual ConfigurationComponent ConfigurationComponent { get; set; } = null!;
        public virtual ConfigurationComponentProduct ConfigurationComponentProduct { get; set; } = null!;
    }
}
