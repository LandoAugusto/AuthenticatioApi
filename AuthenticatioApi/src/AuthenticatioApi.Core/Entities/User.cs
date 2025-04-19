using AuthenticatioApi.Core.Entities.Interfaces;

namespace AuthenticatioApi.Core.Entities
{
    public class User : IIdentityEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProfileId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Code { get; set; }
        public int Status { get; set; }
        public int InclusionUserId { get; set; }
        public DateTime InclusionDate { get; set; }
        public int? LastChangeUserId { get; set; }
        public DateTime? LastChangeDate { get; set; }

    }
}
